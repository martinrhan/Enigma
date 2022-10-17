using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Enigma.Game.SourceGenerator {
    [Generator]
    public class SourceGenerator : ISourceGenerator {
        public void Initialize(GeneratorInitializationContext context) {
        }

        public void Execute(GeneratorExecutionContext context) {
            var templateTypeSymbols =
                context.Compilation.GlobalNamespace.
                GetNamespaceMembers().First(ns => ns.Name == "Enigma").
                GetNamespaceMembers().First(ns => ns.Name == "Game").
                GetTypeMembers().Where(nts => nts.Name.EndsWith("Template"));

            foreach (var templateTypeSymbol in templateTypeSymbols) {
                AddJsonObject(context, templateTypeSymbol);
            }
            context.AddSource("TestGenerated.cs", "");
        }

        private readonly HashSet<INamedTypeSymbol> addedTypes = new();

        private void AddJsonObject(GeneratorExecutionContext context, INamedTypeSymbol templateTypeSymbol) {
            if (addedTypes.Contains(templateTypeSymbol)) return;
            else addedTypes.Add(templateTypeSymbol);

            IEnumerable<IPropertySymbol> propertySymbols =
                templateTypeSymbol.GetMembers().OfType<IPropertySymbol>().
                Where(ps => 
                    ps.DeclaredAccessibility == Accessibility.Public && 
                    !ps.IsStatic && !ps.IsReadOnly && ps.SetMethod.DeclaredAccessibility == Accessibility.Public
                );

            List<CodePieces> list = new();
            foreach (var propertySymbol in propertySymbols) {
                bool useId = !propertySymbol.GetAttributes().Any(
                    attributeData => attributeData.AttributeClass.Name == nameof(DontUseIdAttribute)
                );
                var returnedValue = GetJsonTypeCode(context, ((PropertyDeclarationSyntax)(propertySymbol.DeclaringSyntaxReferences[0].GetSyntax())).Type, useId);
                CodePieces names = new CodePieces();
                names.PropertyTypeName = returnedValue.JsonTypeName;
                names.OriginalPropertyName = propertySymbol.Name;
                names.PropertyName = returnedValue.JsonPropertyName(names.OriginalPropertyName);
                names.PropertyConvertExpression = returnedValue.JsonObjectConvertExpression(names.PropertyName);
                list.Add(names);
            }
            string className = templateTypeSymbol.Name;
            string sourceCode = $@"
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Game.Json{{
    public class {className}JsonObject{{
        "
        + String.Join(Environment.NewLine + "        ", list.
            Select(names => $"public {names.PropertyTypeName} {names.PropertyName} {{ get; set; }}"))
        + $@"
        internal {className} Convert(){{
            return new {className}(){{
                "
                + String.Join("," + Environment.NewLine + "                ", list.
                    Select(names => $"{names.OriginalPropertyName} = {names.PropertyConvertExpression}"))
                + @"
            };
        }
    }
}";
            context.AddSource(className + "JsonObject.cs", sourceCode);
        }

        private ReturnedValue_GetJsonTypeCode GetJsonTypeCode(GeneratorExecutionContext context, TypeSyntax propertyTypeSyntax, bool useId) {
            SemanticModel semanticModel = context.Compilation.GetSemanticModel(propertyTypeSyntax.SyntaxTree);
            TypeInfo typeInfo = semanticModel.GetTypeInfo(propertyTypeSyntax);
            string propertyTypeWrittenName = propertyTypeSyntax.ToString();
            string propertyTypeName = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            switch (propertyTypeSyntax) {
                case PredefinedTypeSyntax predefinedTypeSyntax:
                    return new() {
                        JsonTypeName = propertyTypeWrittenName,
                        JsonPropertyName = name => name,
                        JsonObjectConvertExpression = name => name
                    };
                case IdentifierNameSyntax identifierNameSyntax:
                    if (typeInfo.Type.TypeKind == TypeKind.Enum) {
                        return new() {
                            JsonTypeName = "string",
                            JsonPropertyName = name => name,
                            JsonObjectConvertExpression = name => $"Enum.Parse<{propertyTypeWrittenName}>({name})"
                        };
                    }
                    if (useId) {
                        if (propertyTypeName.StartsWith("Factory<") || propertyTypeName.EndsWith("Template")) {
                            return new() {
                                JsonTypeName = "string",
                                JsonPropertyName = name => name + "Id",
                                JsonObjectConvertExpression = name => $"{propertyTypeWrittenName}.Dictionary[{name}]"
                            };
                        }
                    } else {
                        if (propertyTypeName.EndsWith("Template")) {
                            return GetJsonTypeCode_CustomClass(context, propertyTypeSyntax);
                        }
                        if (propertyTypeName.StartsWith("Factory<")) {
                            throw new NotSupportedException();
                        }
                    }
                    return GetJsonTypeCode_CustomClass(context, propertyTypeSyntax);
                case QualifiedNameSyntax qualifiedNameSyntax:
                    return GetJsonTypeCode_CustomClass(context, propertyTypeSyntax);
                case GenericNameSyntax genericNameSyntax:
                    if (genericNameSyntax.Identifier.Text == "IReadOnlyList") {
                        TypeSyntax argumentTypeSyntax = genericNameSyntax.TypeArgumentList.Arguments[0];
                        var code = GetJsonTypeCode(context, argumentTypeSyntax, useId);
                        return new() {
                            JsonTypeName = $"{code.JsonTypeName}[]",
                            JsonPropertyName = name => name.EndsWith("List") ? code.JsonPropertyName(name.Substring(0, name.Length - 4)) + "List" : throw new InvalidOperationException(),
                            JsonObjectConvertExpression = name => $"({name} ?? Enumerable.Empty<{code.JsonTypeName}>()).Select(jo => {code.JsonObjectConvertExpression("jo")}).ToArray()"
                        };
                    } else {
                        throw new NotSupportedException();
                    }
                default: throw new NotSupportedException();
            }
        }
        private ITypeSymbol GetTypeSymbol(GeneratorExecutionContext context, TypeSyntax typeSyntax) {
            SemanticModel semanticModel = context.Compilation.GetSemanticModel(typeSyntax.SyntaxTree);
            return semanticModel.GetTypeInfo(typeSyntax).Type;
        }
        private ReturnedValue_GetJsonTypeCode GetJsonTypeCode_CustomClass(GeneratorExecutionContext context, TypeSyntax propertyTypeSyntax) {
            SemanticModel semanticModel = context.Compilation.GetSemanticModel(propertyTypeSyntax.SyntaxTree);
            TypeInfo typeInfo = semanticModel.GetTypeInfo(propertyTypeSyntax);
            string propertyTypeWrittenName = propertyTypeSyntax.ToString();
            string propertyTypeName = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            string propertyTypeFullName = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            IMethodSymbol jsonConstructor =
            typeInfo.Type.GetMembers().OfType<IMethodSymbol>().
                Where(methodSymbol => methodSymbol.GetAttributes().
                    Any(attributeData => attributeData.AttributeClass.Name == nameof(JsonConstructorAttribute))
                ).
            FirstOrDefault();
            if (jsonConstructor == null) {
                AddJsonObject(context, (INamedTypeSymbol)(typeInfo.Type));
                return new() {
                    JsonTypeName = propertyTypeName + "JsonObject",
                    JsonPropertyName = name => name,
                    JsonObjectConvertExpression = name => name + ".Convert()"
                };
            } else {
                if (jsonConstructor.DeclaredAccessibility != Accessibility.Public) throw new InvalidOperationException();
                if (!jsonConstructor.IsStatic) throw new InvalidOperationException();
                if (jsonConstructor.Parameters.Length == 1) {
                    return new() {
                        JsonTypeName = jsonConstructor.Parameters[0].Type.ToString(),
                        JsonPropertyName = name => name,
                        JsonObjectConvertExpression = name =>
                        name == propertyTypeWrittenName ? $"{propertyTypeFullName}.{jsonConstructor.Name}({name})" : $"{propertyTypeWrittenName}.{jsonConstructor.Name}({name})"
                    };
                } else {
                    throw new NotSupportedException();
                }
            }
        }

        private struct CodePieces {
            internal string PropertyTypeName { get; set; }
            internal string OriginalPropertyName { get; set; }
            internal string PropertyName { get; set; }
            internal string PropertyConvertExpression { get; set; }
        }

        struct ReturnedValue_GetJsonTypeCode {
            internal string JsonTypeName { get; set; }
            /// <summary>
            /// Conversion from name of the property of original type to name of the property of JsonObject type
            /// </summary>
            internal Func<string, string> JsonPropertyName { get; set; }
            /// <summary>
            /// Conversion from the name of the property of JsonObjectType to the Expression of converting the property to an instance of the original type.
            /// </summary>
            internal Func<string, string> JsonObjectConvertExpression { get; set; }
        }
    }
}