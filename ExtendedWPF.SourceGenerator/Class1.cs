using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedWPF.SourceGenerator {
    [Generator]
    public class SourceGenerator : ISourceGenerator {
        public void Initialize(GeneratorInitializationContext context) {
        }

        public void Execute(GeneratorExecutionContext context) {
            var types = context.Compilation.SourceModule.ReferencedAssemblySymbols.SelectMany(a => {
                try {
                    INamespaceSymbol main = a.Identity.Name.Split('.').Aggregate(a.GlobalNamespace, (s, c) => s.GetNamespaceMembers().Single(m => m.Name.Equals(c)));

                    return GetAllTypes(main);
                } catch {
                    return Enumerable.Empty<ITypeSymbol>();
                }
            });
        }

        private static IEnumerable<ITypeSymbol> GetAllTypes(INamespaceSymbol root) {
            foreach (var namespaceOrTypeSymbol in root.GetMembers()) {
                if (namespaceOrTypeSymbol is INamespaceSymbol @namespace) foreach (var nested in GetAllTypes(@namespace)) yield return nested;

                else if (namespaceOrTypeSymbol is ITypeSymbol type) yield return type;
            }
        }
    }
}