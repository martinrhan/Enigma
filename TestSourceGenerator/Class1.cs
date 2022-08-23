using Microsoft.CodeAnalysis;

namespace TestSourceGenerator {
    [Generator]
    public class SourceGenerator : ISourceGenerator {
        public void Initialize(GeneratorInitializationContext context) {
        }

        public void Execute(GeneratorExecutionContext context) {
            context.AddSource("bbb.cs", "");
        }
    }
}