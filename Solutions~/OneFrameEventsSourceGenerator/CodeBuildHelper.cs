using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OneFrameEventsSourceGenerator
{
    public static class CodeBuildHelper
    {
        public const string Namespace = "ED.DOTS.OneFrameEvents";
        public const string EventInterface = "IOneFrameEvent";
        public const string SystemGroup = "OneFrameEventSimulationSystemGroup";
        
        public static string SystemName(string componentName) => $"{componentName}_System";
        
        public static string RequestName(string componentName) => $"{componentName}_Request";

        public static string GetNamespace(SyntaxNode node)
        {
            while (node != null)
            {
                if (node is NamespaceDeclarationSyntax namespaceDeclaration)
                    return namespaceDeclaration.Name.ToString();
                node = node.Parent;
            }
            return null;
        }
    }
}