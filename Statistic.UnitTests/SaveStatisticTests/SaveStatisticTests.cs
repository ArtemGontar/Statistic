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

        [Fact(Skip = "investigate how to test conumer")]
        public void Consume_ValidData_ShouldSuccess()
        {
            //Arrange
            //var context = ConsumeContext<QuizResultMessage>

            //Act
            //await _consumer.Consume();

            //Assert
        }
    }
}
