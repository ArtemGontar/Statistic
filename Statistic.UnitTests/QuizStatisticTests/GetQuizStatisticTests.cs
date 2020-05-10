using Moq;
using Moq.AutoMock;
using Shared.Persistence.MongoDb;
using Statistic.Application.QuizStatistic.GetLastQuizUserStatistic;
using Statistic.Application.QuizStatistic.GetLastUserQuizStatistic;
using Statistic.Application.QuizStatistic.GetQuizStatistic;
using Statistic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Statistic.UnitTests.QuizStatisticTests
{
    public class GetQuizStatisticTests
    {
        private AutoMocker _autoMocker;
        private readonly Mock<IRepository<QuizStatistic>> _quizStatisticRepositoryMock;

        public GetQuizStatisticTests()
        {
            _autoMocker = new AutoMocker();
            _quizStatisticRepositoryMock = _autoMocker.GetMock<IRepository<QuizStatistic>>();
        }

        [Fact]
        public async Task GetQuizStatisticByQuizIdHandler_ExsistId_ShouldSuccess()
        {
            //Arrange
            var quizId = Guid.NewGuid();
            var query = new GetQuizStatisticQuery() { QuizId = quizId };
            var handler = _autoMocker.CreateInstance<GetQuizStatisticQueryHandler>();
            _quizStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<QuizStatistic>>()))
                .ReturnsAsync(new List<QuizStatistic>() {
                    GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5),
                    GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5),
                    GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5)
                });
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
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
            var query = new GetLastQuizStatisticByUserIdQuery()
            {
                QuizId = quizId,
                UserId = userId
            };
            var handler = _autoMocker.CreateInstance<GetLastQuizStatisticByUserIdQueryHandler>();
            _quizStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<QuizStatistic>>()))
                .ReturnsAsync(new List<QuizStatistic>() {
                    GetQuizStatistic(quizId, userId, 10, 5, 5, 0.5),
                    GetQuizStatistic(quizId, userId, 10, 5, 5, 0.5),
                    GetQuizStatistic(quizId, userId, 10, 5, 5, 0.5)
                });
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(quizId, result.QuizId);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetLastQuizStatisticByQuizIdAndQuizIdHandler_NotExsistIds_ShouldArgumentNullException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var query = new GetLastQuizStatisticByUserIdQuery()
            {

                QuizId = quizId,
                UserId = userId
            };
            var handler = _autoMocker.CreateInstance<GetLastQuizStatisticByUserIdQueryHandler>();
            _quizStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<QuizStatistic>>()))
                .ReturnsAsync(default(List<QuizStatistic>));
            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, CancellationToken.None));

            //Assert
            Assert.Contains($"Quiz statistic for quiz id {quizId} and user id {userId} not found", result.Message);
        }


        private QuizStatistic GetQuizStatistic(Guid quizId, Guid userId, int questionsCount,
            int correctAnswersCount, int wrongAnswersCount, double correctPercent)
        {
            return new QuizStatistic()
            {
                QuizId = quizId,
                UserId = userId,
                QuestionsCount = questionsCount,
                CorrectAnswersCount = correctAnswersCount,
                WrongAnswersCount = wrongAnswersCount,
                CorrectPercent = correctPercent,
                TimeStamp = DateTime.UtcNow
            };
        }
    }
}
