using Moq.AutoMock;
using Statistic.Application.Services;
using Statistic.Application.Views;
using Statistic.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Statistic.UnitTests.ServiceTests
{
    public class QuizStatisticServiceTests
    {
        private AutoMocker _autoMocker;

        private QuizStatisticService _quizStatisticService;
        public QuizStatisticServiceTests()
        {
            _autoMocker = new AutoMocker();
            _quizStatisticService = _autoMocker.CreateInstance<QuizStatisticService>();
        }


        [Fact]
        public void GetUserStatistics_ValidData_ShouldSuccess()
        {
            //Arrange
            var quizStatisticList = GetQuizStatisticList(Guid.NewGuid());

            //Act
            var result = _quizStatisticService.GetQuizStatistics(quizStatisticList);

            //Assert
            Assert.NotStrictEqual(GetCorrectQuizStatisticView(), result);
        }

        [Fact]
        public void GetUserStatistics_EmptyArray_Should()
        {
            //Arrange
            var quizStatisticList = new List<QuizStatistic>();

            //Act
            var result = _quizStatisticService.GetQuizStatistics(quizStatisticList);

            //Assert
            Assert.NotStrictEqual(new QuizStatisticView(), result);
        }

        private List<QuizStatistic> GetQuizStatisticList(Guid quizId)
        {
            return new List<QuizStatistic>() {
                GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5m),
                GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5m),
                GetQuizStatistic(quizId, Guid.NewGuid(), 10, 5, 5, 0.5m)
            };
        }

        private QuizStatisticView GetCorrectQuizStatisticView()
        {
            return new QuizStatisticView()
            {
                PassedPercent = 0.5m,
                TimeToSolved = new TimeSpan(1, 14, 18).ToString(),
                QuizResultChartView = new QuizResultChartView()
                {
                    CorrectAnswers = 15,
                    FaliedAnswers = 15,
                    TotalAnswers = 30
                },
            };
        }

        private QuizStatistic GetQuizStatistic(Guid quizId, Guid userId, int questionsCount,
            int correctAnswersCount, int wrongAnswersCount, decimal correctPercent)
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
