using Moq.AutoMock;
using Statistic.Application.Consumers;
using System.Threading.Tasks;
using Xunit;

namespace Statistic.UnitTests.SaveStatisticTests
{
    public class SaveStatisticTests
    {
        private AutoMocker _autoMocker;

        private readonly QuizResultConsumer _consumer;
        
        public SaveStatisticTests()
        {
            _autoMocker = new AutoMocker();

            _consumer = _autoMocker.CreateInstance<QuizResultConsumer>();

        }
    }
}
