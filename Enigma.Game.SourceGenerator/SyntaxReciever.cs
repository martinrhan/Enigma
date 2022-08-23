using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.Game.SourceGenerator {
    internal class SyntaxReciever : ISyntaxReceiver {
        internal readonly List<TypeDeclarationSyntax> TypeDeclarationSyntaxList = new ();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {
            if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax) {
                TypeDeclarationSyntaxList.Add(typeDeclarationSyntax);
            }
        }
    }
}
