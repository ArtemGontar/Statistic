using MassTransit;
using System;
using System.Threading.Tasks;
using Shared.Bus.Messages;

namespace Statistic.Application.Consumers
{
    public class QuizResultConsumer :
        IConsumer<QuizResultMessage>
    {
        public async Task Consume(ConsumeContext<QuizResultMessage> context)
        {
            await Console.Out.WriteLineAsync($"Quiz result: {context.Message.CorrectPercent}");
        }
    }
}
