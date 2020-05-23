using Moq;
using Moq.AutoMock;
using Shared.Persistence.MongoDb;
using Statistic.Application.QuizStatistic.GetLastQuizUserStatistic;
using Statistic.Application.QuizStatistic.GetLastUserQuizStatistic;
using Statistic.Application.QuizStatistic.GetQuizStatistic;
using Statistic.Application.Services;
using Statistic.Application.Views;
using Statistic.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Statistic.UnitTests.QuizStatisticTests
{
    public class GetQuizStatisticTests
    {
        private AutoMocker _autoMocker;
        private readonly Mock<IRepository<QuizStatistic>> _quizStatisticRepositoryMock;
        private readonly Mock<IQuizStatisticService> _quizStatisticServiceMock;

        public GetQuizStatisticTests()
        {
            _autoMocker = new AutoMocker();
            _quizStatisticRepositoryMock = _autoMocker.GetMock<IRepository<QuizStatistic>>();
            _quizStatisticServiceMock = _autoMocker.GetMock<IQuizStatisticService>();
        }

        [Fact]
        public async Task GetQuizStatisticByQuizIdHandler_ExsistId_ShouldSuccess()
        {
            //Arrange
            var quizId = Guid.NewGuid();
            var query = new GetQuizStatisticQuery() { QuizId = quizId };
            var handler = _autoMocker.CreateInstance<GetQuizStatisticQueryHandler>();
            _quizStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<QuizStatistic>>()))
                .ReturnsAsync(GetQuizStatisticList(quizId));
            _quizStatisticServiceMock.Setup(x => x.GetQuizStatistics(It.IsAny<IEnumerable<QuizStatistic>>()))
                .Returns(GetQuizStatisticView());
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetQuizStatisticByQuizIdHandler_NotExsistId_ShouldArgumentNullException()
        {
            //Arrange
            var quizId = Guid.NewGuid();
            var query = new GetQuizStatisticQuery() { QuizId = quizId };
            var handler = _autoMocker.CreateInstance<GetQuizStatisticQueryHandler>();
            _quizStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<QuizStatistic>>()))
                .ReturnsAsync(default(List<QuizStatistic>));
            
            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, CancellationToken.None));

            //Assert
            Assert.Contains($"Quiz statistic for quiz {quizId} not found", result.Message);
        }

        [Fact]
        public async Task GetLastQuizStatisticByQuizIdAndQuizIdHandler_ExsistIds_ShouldSuccess()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var query = new GetQuizStatisticByUserIdQuery()
            {
                QuizId = quizId,
                UserId = userId
            };
            var handler = _autoMocker.CreateInstance<GetQuizStatisticByUserIdQueryHandler>();
            _quizStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<QuizStatistic>>()))
                .ReturnsAsync(GetQuizStatisticList(quizId));
            _quizStatisticServiceMock.Setup(x => x.GetQuizStatistic(It.IsAny<QuizStatistic>()))
                .Returns(GetQuizStatisticView());

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLastQuizStatisticByQuizIdAndQuizIdHandler_NotExsistIds_ShouldArgumentNullException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var query = new GetQuizStatisticByUserIdQuery()
            {

                QuizId = quizId,
                UserId = userId
            };
            var handler = _autoMocker.CreateInstance<GetQuizStatisticByUserIdQueryHandler>();
            _quizStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<QuizStatistic>>()))
                .ReturnsAsync(default(List<QuizStatistic>));
            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, CancellationToken.None));

            //Assert
            Assert.Contains($"Quiz statistic for quiz id {quizId} and user id {userId} not found", result.Message);
        }

        private List<QuizStatistic> GetQuizStatisticList(Guid quizId)
        {
            return new List<QuizStatistic>() {
                GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5),
                GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5),
                GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5)
            };
        }

        private QuizStatisticView GetQuizStatisticView()
        {
            return new QuizStatisticView()
            {
                PassedPercent = 2,
                QuizResultChart = new QuizResultChartView()
                {
                    CorrectAnswers = 2,
                    FaliedAnswers = 1,
                    TotalAnswers = 3
                },
                TimeToSolved = new TimeSpan(1, 14, 18)
            };
        }

        private QuizStatistic GetQuizStatistic(Guid quizId, Guid userId, int questionsCount,
            int correctAnswersCount, int wrongAnswersCount, double correctPercent)
        {
            return new QuizStatistic()
            {
                QuizId = quizId,
                UserId = userId,
                TotalAnswersCount = questionsCount,
                CorrectAnswersCount = correctAnswersCount,
                FailedAnswersCount = wrongAnswersCount,
                CorrectPercent = correctPercent,
                TimeStamp = DateTime.UtcNow
            };
        }
    }
}
