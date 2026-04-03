using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OneFrameEventsSourceGenerator
{
    public class SyntaxReceiver : ISyntaxReceiver
    {
        private FileLogger Logger { get; }
        
        public readonly List<StructDeclarationSyntax> OneFrameEvents = new List<StructDeclarationSyntax>();

        public SyntaxReceiver(FileLogger logger)
        {
            Logger = logger;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is StructDeclarationSyntax structNode)) return;
            
            if (IsOneFrameEvent(structNode))
            {
                OneFrameEvents.Add(structNode);
                
                var component = structNode.Identifier.Text;
                var @namespace = CodeBuildHelper.GetNamespace(structNode);
                Logger.Info($"{GetType().Name}: founded {CodeBuildHelper.EventInterface}: {@namespace}.{component}");
            }
        }

        private bool IsOneFrameEvent(StructDeclarationSyntax node)
        {
            if (node.BaseList == null) return false;
            foreach (var type in node.BaseList.Types)
                if (type.Type is IdentifierNameSyntax name)
                    if (name.Identifier.Text == CodeBuildHelper.EventInterface)
                        return true;
            return false;
        }
    }
}