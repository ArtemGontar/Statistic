using Moq;
using Moq.AutoMock;
using Shared.Persistence.MongoDb;
using Statistic.Application.Statistic.GetUserStatistic;
using Statistic.Application.UserStatistic.GetUserStatistic;
using Statistic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Statistic.UnitTests.UserStatisticTests
{
    public class GetUserStatisticTests
    {
        private AutoMocker _autoMocker;
        private readonly Mock<IRepository<UserStatistic>> _userStatisticRepositoryMock;

        public GetUserStatisticTests()
        {
            _autoMocker = new AutoMocker();
            _userStatisticRepositoryMock = _autoMocker.GetMock<IRepository<UserStatistic>>();
        }
        
        [Fact]
        public async Task GetUserStatisticByQuizIdHandler_ExsistId_ShouldSuccess()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserStatisticQuery() { UserId = userId };
            var handler = _autoMocker.CreateInstance<GetUserStatisticQueryHandler>();
            _userStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<UserStatistic>>()))
                .ReturnsAsync(new List<UserStatistic>() {
                    GetUserStatistic(userId, Guid.NewGuid(), 10, 5, 5, 0.5),
                    GetUserStatistic(userId, Guid.NewGuid(), 10, 5, 5, 0.5),
                    GetUserStatistic(userId, Guid.NewGuid(), 10, 5, 5, 0.5)
                });
            //Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetUserStatisticByUserIdHandler_NotExsistId_ShouldArgumentNullException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserStatisticQuery() { UserId = userId };
            var handler = _autoMocker.CreateInstance<GetUserStatisticQueryHandler>();
            _userStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<UserStatistic>>()))
                .ReturnsAsync(default(List<UserStatistic>));
            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, CancellationToken.None));

            //Assert
            Assert.Contains($"User statistic for user {userId} not found", result.Message);
        }

        [Fact]
        public async Task GetLastUserStatisticByUserIdAndQuizIdHandler_ExsistIds_ShouldSuccess()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var query = new GetLastUserStatisticByQuizIdQuery() { 
                UserId = userId,
                QuizId = quizId
            };
            var handler = _autoMocker.CreateInstance<GetLastUserStatisticByQuizIdQueryHandler>();
            _userStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<UserStatistic>>()))
                .ReturnsAsync(new List<UserStatistic>() { 
                    GetUserStatistic(userId, quizId, 10, 5, 5, 0.5),
                    GetUserStatistic(userId, quizId, 10, 5, 5, 0.5),
                    GetUserStatistic(userId, quizId, 10, 5, 5, 0.5)
                });
            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(quizId, result.QuizId);
        }

        [Fact]
        public async Task GetLastUserStatisticByUserIdAndQuizIdHandler_NotExsistIds_ShouldArgumentNullException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var query = new GetLastUserStatisticByQuizIdQuery()
            {
                UserId = userId,
                QuizId = quizId
            };
            var handler = _autoMocker.CreateInstance<GetLastUserStatisticByQuizIdQueryHandler>();
            _userStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<UserStatistic>>()))
                .ReturnsAsync(default(List<UserStatistic>));
            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, CancellationToken.None));

            //Assert
            Assert.Contains($"User statistic for user id {userId} and quiz id {quizId} not found", result.Message);
        }


        private UserStatistic GetUserStatistic(Guid userId, Guid quizId, int questionsCount,
            int correctAnswersCount, int wrongAnswersCount, double correctPercent)
        {
            return new UserStatistic() {
                UserId = userId,
                QuizId = quizId,
                QuestionsCount = questionsCount,
                CorrectAnswersCount = correctAnswersCount,
                WrongAnswersCount = wrongAnswersCount,
                CorrectPercent = correctPercent,
                TimeStamp = DateTime.UtcNow
            };
        }
    }
}
