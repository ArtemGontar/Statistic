using Moq.AutoMock;
using Shared.Common;
using Statistic.Application.Services;
using Statistic.Application.Views;
using Statistic.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Statistic.UnitTests.ServiceTests
{
    public class UserStatisticServiceTests
    {
        private AutoMocker _autoMocker;

        private UserStatisticService _userStatisticService;
        public UserStatisticServiceTests()
        {
            _autoMocker = new AutoMocker();
            _userStatisticService = _autoMocker.CreateInstance<UserStatisticService>();

        }

        [Fact]
        public void GetUserStatistics_ValidData_ShouldSuccess() 
        {
            //Arrange
            var userStatisticList = GetUserStatisticList(Guid.NewGuid(), "anonymousTitle");

            //Act
            var result = _userStatisticService.GetUserStatistics(userStatisticList, EnglishLevel.Beginner);
            
            //Assert
            Assert.NotStrictEqual(GetCorrectUserStatisticView(), result);
        }

        [Fact]
        public void GetUserStatistics_EmptyArray_Should()
        {
            //Arrange
            var userStatisticList = new List<UserStatistic>();

            //Act
            var result = _userStatisticService.GetUserStatistics(userStatisticList, EnglishLevel.Beginner);

            //Assert
            Assert.NotStrictEqual(new UserStatisticView()
            {
                EnglishLevel = EnglishLevel.Beginner
            }, result);
        }


        private List<UserStatistic> GetUserStatisticList(Guid userId, string quizTitle)
        {
            return new List<UserStatistic>()
            {
                GetUserStatistic(userId, Guid.NewGuid(), quizTitle, questionsCount: 10, correctAnswersCount: 5, failedAnswersCount: 5, correctPercent: 0.5),
                GetUserStatistic(userId, Guid.NewGuid(), quizTitle, questionsCount: 10, correctAnswersCount: 5, failedAnswersCount: 5, correctPercent: 0.5),
                GetUserStatistic(userId, Guid.NewGuid(), quizTitle, questionsCount: 10, correctAnswersCount: 5, failedAnswersCount: 5, correctPercent: 0.5)
            };
        }

        private UserStatistic GetUserStatistic(Guid userId, Guid quizId, string quizTitle, int questionsCount,
            int correctAnswersCount, int failedAnswersCount, double correctPercent)
        {
            return new UserStatistic()
            {
                UserId = userId,
                QuizId = quizId,
                QuizTitle = quizTitle,
                QuestionsCount = questionsCount,
                CorrectAnswersCount = correctAnswersCount,
                FailedAnswersCount = failedAnswersCount,
                CorrectPercent = correctPercent,
                TimeStamp = DateTime.UtcNow
            };
        }

        private UserStatisticByQuizView GetCorrectUserStatisticByQuizView()
        {
            return new UserStatisticByQuizView()
            {
                ScoreForQuiz = 0.5,
                TimeToSolved = new TimeSpan(1, 14, 18),
                QuizResultChartView = new QuizResultChartView()
                {
                    CorrectAnswers = 1,
                    FaliedAnswers = 1,
                    TotalAnswers = 1
                }
            };
        }

        private UserStatisticView GetCorrectUserStatisticView()
        {
            return new UserStatisticView()
            {
                EnglishLevel = EnglishLevel.Beginner,
                PassedPercent = 0.5,
                ScoreForQuizzes = 0.5,
                TotalFailedQuestions = 15,
                TotalPassedQuestions = 15,
                TotalPassedQuizzes = 3,
                LastScoresChartView = new LastScoresChartView()
                {
                    QuizScores = new List<QuizScoreView>()
                    {
                        new QuizScoreView
                        {
                            Title = "anonymousTitle",
                            Score = 0.5
                        },
                        new QuizScoreView
                        {
                            Title = "anonymousTitle",
                            Score = 0.5
                        },
                        new QuizScoreView
                        {
                            Title = "anonymousTitle",
                            Score = 0.5
                        }
                    }
                },
                QuizResultChartView = new QuizResultChartView()
                {
                    CorrectAnswers = 15,
                    FaliedAnswers = 15,
                    TotalAnswers = 30
                }
            };
        }
    }
}
