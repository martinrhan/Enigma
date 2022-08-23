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
            context.RegisterForSyntaxNotifications(() => new SyntaxReciever());

        }

        public void Execute(GeneratorExecutionContext context) {
            List<TypeDeclarationSyntax> list = ((SyntaxReciever)(context.SyntaxReceiver)).TypeDeclarationSyntaxList;
            Dictionary<string, TypeDeclarationSyntax> map = new();

            foreach (TypeDeclarationSyntax typeDeclarationSyntax in list) {
                var namedTypeSymbol = context.Compilation.GetSemanticModel(typeDeclarationSyntax.SyntaxTree).GetDeclaredSymbol(typeDeclarationSyntax);
                string typeFullName = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!map.ContainsKey(typeFullName)) {
                    map.Add(typeFullName, typeDeclarationSyntax);
                }
            }

            foreach (TypeDeclarationSyntax typeDeclarationSyntax in list) {
                if (typeDeclarationSyntax.Identifier.Text.EndsWith("Template")) {
                    AddJsonObject(context, map, typeDeclarationSyntax);
                }
            }
            context.AddSource("aaa.cs", "");
        }

        private readonly HashSet<string> addedJsonObjectTypes = new HashSet<string>();

        private void AddJsonObject(GeneratorExecutionContext context, IReadOnlyDictionary<string, TypeDeclarationSyntax> map, TypeDeclarationSyntax typeDeclarationSyntax) {
            SemanticModel semanticModel = context.Compilation.GetSemanticModel(typeDeclarationSyntax.SyntaxTree);
            INamedTypeSymbol namedTypeSymbol = semanticModel.GetDeclaredSymbol(typeDeclarationSyntax);
            if (addedJsonObjectTypes.Contains(namedTypeSymbol.ToString())) {
                return;
            }
            addedJsonObjectTypes.Add(namedTypeSymbol.ToString());
            List<Names> list = new();
            foreach (var propertyDeclarationSyntax in typeDeclarationSyntax.Members.
                OfType<PropertyDeclarationSyntax>().
                Where(syntax =>
                    syntax.Modifiers.Any(token => token.IsKind(SyntaxKind.PublicKeyword)) &&
                    !syntax.Modifiers.Any(token => token.IsKind(SyntaxKind.StaticKeyword)) &&
                    syntax.AccessorList.Accessors.Count == 2 &&
                    !syntax.AccessorList.Accessors[1].Modifiers.Any()
                )
            ) {
                bool useId = ! propertyDeclarationSyntax.AttributeLists.Any(
                    attributeList => {
                        var attribute = attributeList.Attributes[0];
                        return context.Compilation.GetSemanticModel(attribute.SyntaxTree).GetTypeInfo(attribute).Type.ToDisplayString() == typeof(DontUseIdAttribute).FullName;
                    }
                );
                var returnedValue = GetJsonTypeCode(context, map, propertyDeclarationSyntax.Type, useId);
                Names names = new Names();
                names.TypeName = returnedValue.JsonTypeName;
                names.OriginalPropertyName = propertyDeclarationSyntax.Identifier.ToString();
                names.PropertyName = returnedValue.JsonPropertyName(names.OriginalPropertyName);
                names.TypeConvertExpression = returnedValue.JsonObjectConvertExpression(names.PropertyName);
                list.Add(names);
            }
            string className = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            string sourceCode = $@"
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Game.Json{{
    public class {className}JsonObject{{
        "
        + String.Join(Environment.NewLine + "        ", list.
            Select(names => $"public {names.TypeName} {names.PropertyName} {{ get; set; }}"))
        + $@"
        public {className} Convert(){{
            return new {className}(){{
                "
                + String.Join("," + Environment.NewLine + "                ", list.
                    Select(names => $"{names.OriginalPropertyName} = {names.TypeConvertExpression}"))
                + @"
            };
        }
    }
}";
            context.AddSource(className + "JsonObject.cs", sourceCode);
        }
        private struct Names {
            internal string TypeName { get; set; }
            internal string OriginalPropertyName { get; set; }
            internal string PropertyName { get; set; }
            internal string TypeConvertExpression { get; set; }
        }

        private ReturnedValue_GetJsonTypeCode GetJsonTypeCode(
            GeneratorExecutionContext context, IReadOnlyDictionary<string, TypeDeclarationSyntax> map, TypeSyntax propertyTypeSyntax, bool useId) {
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
                            return GetJsonTypeCode_CustomClass(context, map, propertyTypeSyntax);
                        }
                        if (propertyTypeName.StartsWith("Factory<")) {
                            throw new NotSupportedException();
                        }
                    }
                    return GetJsonTypeCode_CustomClass(context, map, propertyTypeSyntax);
                case QualifiedNameSyntax qualifiedNameSyntax:
                    return GetJsonTypeCode_CustomClass(context, map, propertyTypeSyntax);
                case GenericNameSyntax genericNameSyntax:
                    if (genericNameSyntax.Identifier.Text == "IReadOnlyList") {
                        TypeSyntax argumentTypeSyntax = genericNameSyntax.TypeArgumentList.Arguments[0];
                        var code = GetJsonTypeCode(context, map, argumentTypeSyntax, useId);
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

        private ReturnedValue_GetJsonTypeCode GetJsonTypeCode_CustomClass(GeneratorExecutionContext context, IReadOnlyDictionary<string, TypeDeclarationSyntax> map, TypeSyntax propertyTypeSyntax) {
            SemanticModel semanticModel = context.Compilation.GetSemanticModel(propertyTypeSyntax.SyntaxTree);
            TypeInfo typeInfo = semanticModel.GetTypeInfo(propertyTypeSyntax);
            string propertyTypeWrittenName = propertyTypeSyntax.ToString();
            string propertyTypeName = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            string propertyTypeFullName = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            TypeDeclarationSyntax typeDeclartionSyntax;
            if (map.TryGetValue(propertyTypeFullName, out typeDeclartionSyntax)) {
                MethodDeclarationSyntax jsonConstructor =
                typeDeclartionSyntax.Members.OfType<MethodDeclarationSyntax>().
                    Where(syntax => syntax.AttributeLists.
                        Any(
                            attributeList => {
                                var attribute = attributeList.Attributes[0];
                                return context.Compilation.GetSemanticModel(attribute.SyntaxTree).GetTypeInfo(attribute).Type.ToDisplayString() == typeof(JsonConstructorAttribute).FullName;
                            }
                        )
                    ).FirstOrDefault();
                if (jsonConstructor == null) {
                    AddJsonObject(context, map, typeDeclartionSyntax);
                    return new() {
                        JsonTypeName = propertyTypeName + "JsonObject",
                        JsonPropertyName = name => name,
                        JsonObjectConvertExpression = name => name + ".Convert()"
                    };
                } else {
                    if (!jsonConstructor.Modifiers.Any(token => token.IsKind(SyntaxKind.PublicKeyword))) throw new InvalidOperationException();
                    if (!jsonConstructor.Modifiers.Any(token => token.IsKind(SyntaxKind.StaticKeyword))) throw new InvalidOperationException();
                    var syntaxList = jsonConstructor.ParameterList.Parameters;
                    if (syntaxList.Count == 1) {
                        return new() {
                            JsonTypeName = syntaxList[0].Type.ToString(),
                            JsonPropertyName = name => name,
                            JsonObjectConvertExpression = name => name == propertyTypeWrittenName ?
                            $"{propertyTypeFullName}.{jsonConstructor.Identifier.Text}({name})" :
                            $"{propertyTypeWrittenName}.{jsonConstructor.Identifier.Text}({name})"
                        };
                    } else {
                        throw new NotSupportedException();
                    }
                }
            } else {
                throw new NotSupportedException();
            }
        }

        struct ReturnedValue_GetJsonTypeCode {
            internal string JsonTypeName { get; set; }
            internal Func<string, string> JsonPropertyName { get; set; }
            internal Func<string, string> JsonObjectConvertExpression { get; set; }
        }
    }
}