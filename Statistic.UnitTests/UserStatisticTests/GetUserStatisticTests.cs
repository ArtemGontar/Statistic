using Moq;
using Moq.AutoMock;
using Shared.Persistence.MongoDb;
using Statistic.Application.Services;
using Statistic.Application.Statistic.GetUserStatistic;
using Statistic.Application.UserStatistic.GetUserStatistic;
using Statistic.Application.Views;
using Statistic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Shared.Common;

namespace Statistic.UnitTests.UserStatisticTests
{
    public class GetUserStatisticTests
    {
        private AutoMocker _autoMocker;
        private readonly Mock<IRepository<UserStatistic>> _userStatisticRepositoryMock;
        private readonly Mock<IUserStatisticService> _userStatisticServiceMock;

        public GetUserStatisticTests()
        {
            _autoMocker = new AutoMocker();
            _userStatisticRepositoryMock = _autoMocker.GetMock<IRepository<UserStatistic>>();
            _userStatisticServiceMock = _autoMocker.GetMock<IUserStatisticService>();
        }
        
        [Fact]
        public async Task GetUserStatisticByQuizIdHandler_ExsistId_ShouldSuccess()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserStatisticQuery() { UserId = userId };
            var handler = _autoMocker.CreateInstance<GetUserStatisticQueryHandler>();
            _userStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<UserStatistic>>()))
                .ReturnsAsync(GetUserStatisticList(userId));
            _userStatisticServiceMock.Setup(x => x.GetUserStatistics(It.IsAny<IEnumerable<UserStatistic>>(), It.IsAny<EnglishLevel>()))
                .Returns(GetUserStatisticView());

            //Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            Assert.NotNull(result);
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
            var query = new GetUserStatisticByQuizQuery() { 
                UserId = userId,
                QuizId = quizId
            };
            var handler = _autoMocker.CreateInstance<GetUserStatisticByQuizQueryHandler>();
            _userStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<UserStatistic>>()))
                .ReturnsAsync(GetUserStatisticList(userId));
            _userStatisticServiceMock.Setup(x => x.GetUserStatistic(It.IsAny<UserStatistic>(), It.IsAny<EnglishLevel>()))
                .Returns(GetUserStatisticByQuizView());

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLastUserStatisticByUserIdAndQuizIdHandler_NotExsistIds_ShouldArgumentNullException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var query = new GetUserStatisticByQuizQuery()
            {
                UserId = userId,
                QuizId = quizId
            };
            var handler = _autoMocker.CreateInstance<GetUserStatisticByQuizQueryHandler>();
            _userStatisticRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<UserStatistic>>()))
                .ReturnsAsync(default(List<UserStatistic>));
            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, CancellationToken.None));

            //Assert
            Assert.Contains($"User statistic for user id {userId} and quiz id {quizId} not found", result.Message);
        }

        private List<UserStatistic> GetUserStatisticList(Guid userId)
        {
            return new List<UserStatistic>()
            {
                GetUserStatistic(userId, Guid.NewGuid(), 10, 5, 5, 0.5m),
                GetUserStatistic(userId, Guid.NewGuid(), 10, 5, 5, 0.5m),
                GetUserStatistic(userId, Guid.NewGuid(), 10, 5, 5, 0.5m)
            };
        }

        private UserStatisticView GetUserStatisticView()
        {
            return new UserStatisticView()
            {
                EnglishLevel = EnglishLevel.Beginner,
                PassedPercent = 2,
                ScoreForQuizzes = 2.0m,
                TotalFailedQuestions = 2,
                TotalPassedQuestions = 1,
                TotalPassedQuizzes = 1,
                LastScoresChartView = new LastScoresChartView() { 
                    QuizScores = new List<QuizScoreView>() { 
                    }
                },
                QuizResultChartView = new QuizResultChartView()
                {
                    CorrectAnswers = 1,
                    FaliedAnswers = 1,
                    TotalAnswers = 1
                }
            };
        }

        private UserStatisticByQuizView GetUserStatisticByQuizView()
        {
            return new UserStatisticByQuizView()
            {
                ScoreForQuiz = 2,
                TimeToSolved = new TimeSpan(1, 14, 18).ToString(),
                QuizResultChartView = new QuizResultChartView()
                {
                    CorrectAnswers = 1,
                    FaliedAnswers = 1,
                    TotalAnswers = 1
                }
            };
        }

        private UserStatistic GetUserStatistic(Guid userId, Guid quizId, int questionsCount,
            int correctAnswersCount, int wrongAnswersCount, decimal correctPercent)
        {
            return new UserStatistic() {
                UserId = userId,
                QuizId = quizId,
                QuestionsCount = questionsCount,
                CorrectAnswersCount = correctAnswersCount,
                FailedAnswersCount = wrongAnswersCount,
                CorrectPercent = correctPercent,
                TimeStamp = DateTime.UtcNow
            };
        }
    }
}
