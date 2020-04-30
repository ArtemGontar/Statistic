using System;
using System.Threading.Tasks;
using MassTransit;
using Shared.Bus.Messages;

namespace Statistic.Application.Consumers
{
    public class DeleteChapterConsumer :
        IConsumer<DeleteChapterMessage>
    {
        public async Task Consume(ConsumeContext<DeleteChapterMessage> context)
        {
            await Console.Out.WriteLineAsync($"Delete chapter: {context.Message.ChapterId}");
        }
    }
}
