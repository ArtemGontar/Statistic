using Shared.Common;
using Statistic.Application.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Statistic.Application.Services
{
    using Domain;

    public interface IUserStatisticService
    {
        UserStatisticView GetUserStatistics(IEnumerable<UserStatistic> userStatistics, EnglishLevel englishLevel);
        UserStatisticByQuizView GetUserStatistic(UserStatistic userStatistic, EnglishLevel englishLevel);
    }

    public class UserStatisticService : IUserStatisticService
    {
        public UserStatisticService()
        {

        }

        public UserStatisticView GetUserStatistics(IEnumerable<UserStatistic> userStatistics, EnglishLevel englishLevel)
        {
            if(userStatistics == null || !userStatistics.Any())
            {
                return new UserStatisticView()
                {
                    EnglishLevel = EnglishLevel.Beginner
                };
            }
            var userStatisticView = new UserStatisticView()
            {
                TotalPassedQuizzes = userStatistics.Select(x => x.QuizId).Distinct().Count(),
                ScoreForQuizzes = userStatistics.Sum(x => x.CorrectPercent) / userStatistics.Count(),
                EnglishLevel = englishLevel,
                LastScoresChartView = new LastScoresChartView
                {
                    QuizScores = userStatistics.Take(5).Select(x => new QuizScoreView { Title = x.QuizTitle, Score = x.CorrectPercent })
                },
                QuizResultChartView = new QuizResultChartView
                {
                    CorrectAnswers = userStatistics.Sum(x => x.CorrectAnswersCount),
                    FaliedAnswers = userStatistics.Sum(x => x.FailedAnswersCount),
                    TotalAnswers = userStatistics.Sum(x => x.QuestionsCount)
                },
                PassedPercent = userStatistics.Sum(x => x.CorrectPercent) / userStatistics.Count(),
                TotalPassedQuestions = userStatistics.Sum(x => x.CorrectAnswersCount),
                TotalFailedQuestions = userStatistics.Sum(x => x.FailedAnswersCount),

            };
            return userStatisticView;
        }

        public UserStatisticByQuizView GetUserStatistic(UserStatistic userStatistic, EnglishLevel englishLevel)
        {
            var userStatisticView = new UserStatisticByQuizView()
            {
                ScoreForQuiz = userStatistic.CorrectPercent,
                TimeToSolved = new TimeSpan(1, 14, 18),
                EnglishLevel = englishLevel,
                QuizResultChartView = new QuizResultChartView
                {
                    CorrectAnswers = userStatistic.CorrectAnswersCount,
                    FaliedAnswers = userStatistic.FailedAnswersCount,
                    TotalAnswers = userStatistic.QuestionsCount
                }            
            };
            return userStatisticView;
        }
    }
}
