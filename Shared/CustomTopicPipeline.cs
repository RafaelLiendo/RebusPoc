using Rebus.Config;
using Rebus.Pipeline;
using Rebus.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using Rebus.Pipeline.Receive;
using System.Threading.Tasks;

namespace Shared
{
    public static class CustomTopicPipelineExtension
    {
        public static OptionsConfigurer UseCustomTopicPipeline(this OptionsConfigurer c)
        {
            c.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();
                var step = new CustomTopicPipeline();
                return new PipelineStepInjector(pipeline)
                    .OnReceive(step, PipelineRelativePosition.Before, typeof(DeserializeIncomingMessageStep));
            });

            return c;
        }
    }

    [StepDocumentation("Automatically copies account ID from messages into a header if possible")]
    public class CustomTopicPipeline : IIncomingStep
    {
        public async Task Process(IncomingStepContext context, Func<Task> next)
        {
            await next();
        }
    }
}
