using System;
using Microsoft.CodeAnalysis;

namespace OneFrameEventsSourceGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        private FileLogger Logger { get; } = new FileLogger();

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver(Logger));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                if (!(context.SyntaxReceiver is SyntaxReceiver receiver)) return;

                foreach (var node in receiver.OneFrameEvents)
                {
                    SimulationEventSourceGeneratorUtility.Generate(node, context, Logger);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

    }
}