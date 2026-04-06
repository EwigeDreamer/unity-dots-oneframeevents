using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OneFrameEventsSourceGenerator
{
    public class SyntaxReceiver : ISyntaxReceiver
    {
        private FileLogger Logger { get; }
        
        public readonly List<TypeDeclarationSyntax> OneFrameEvents = new List<TypeDeclarationSyntax>();

        public SyntaxReceiver(FileLogger logger)
        {
            Logger = logger;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is TypeDeclarationSyntax typeNode)) return;
            if (!DerivesIOneFrameEvent(typeNode)) return;

            var name = typeNode.Identifier.Text;
            var @namespace = CodeBuildHelper.GetNamespace(typeNode);
            var fullName = CodeBuildHelper.GetFullName(@namespace, name);
            
            Logger.Info($"{GetType().Name}: Founded {CodeBuildHelper.EventInterface}: {fullName}");

            bool canAdd = true;
            
            if (!(typeNode is StructDeclarationSyntax))
            {
                Logger.Error($"{GetType().Name}: The type is not a structure. name: {fullName}, kind: {typeNode.Kind()}");
                canAdd = false;
            }
            
            if (!DerivesIComponentData(typeNode))
            {
                Logger.Error($"{GetType().Name}: The type does not inherit the {CodeBuildHelper.ComponentInterface} interface. name: {fullName}");
                canAdd = false;
            }
            
            if (canAdd)
            {
                OneFrameEvents.Add(typeNode);
                Logger.Info($"{GetType().Name}: Added: {fullName}");
            }
        }

        private static bool DerivesIOneFrameEvent(TypeDeclarationSyntax node)
        {
            if (node.BaseList == null) return false;
            foreach (var type in node.BaseList.Types)
                if (type.Type is IdentifierNameSyntax name)
                    if (name.Identifier.Text.Equals(CodeBuildHelper.EventInterface))
                        return true;
            return false;
        }

        private static bool DerivesIComponentData(TypeDeclarationSyntax node)
        {
            if (node.BaseList == null) return false;
            foreach (var type in node.BaseList.Types)
                if (type.Type is IdentifierNameSyntax name)
                    if (name.Identifier.Text.Equals(CodeBuildHelper.ComponentInterface))
                        return true;
            return false;
        }
    }
}