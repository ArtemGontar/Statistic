using System.Collections.Generic;
using System.Threading.Tasks;
using Statistic.Application.Views;

namespace Statistic.Application.Services
{
    using Domain;
    using System;
    using System.Linq;

    public interface IQuizStatisticService
    {
        QuizStatisticView GetQuizStatistics(IEnumerable<QuizStatistic> quizStatistics);

        QuizStatisticView GetQuizStatistic(QuizStatistic quizStatistic);
    }

    public class QuizStatisticService : IQuizStatisticService
    {
        public QuizStatisticService()
        {

        }

        public QuizStatisticView GetQuizStatistics(IEnumerable<QuizStatistic> quizStatistics)
        {
            if (quizStatistics == null || !quizStatistics.Any())
            {
                return new QuizStatisticView();
            }
            var quizStatisticView = new QuizStatisticView() { 
                PassedPercent = quizStatistics.Sum(x => x.CorrectPercent) / quizStatistics.Count(),
                TimeToSolved = new TimeSpan(1, 14, 18),
                QuizResultChartView = new QuizResultChartView
                {
                    CorrectAnswers = quizStatistics.Sum(x => x.CorrectAnswersCount),
                    FaliedAnswers = quizStatistics.Sum(x => x.FailedAnswersCount),
                    TotalAnswers = quizStatistics.Sum(x => x.TotalAnswersCount)
                },
                LastScoresChartView = new LastScoresChartView
                {
                    QuizScores = quizStatistics.Take(5).Select(x => new QuizScoreView { Title = x.UserName, Score = x.CorrectPercent })
                },
            };
            return quizStatisticView;
        }

        public QuizStatisticView GetQuizStatistic(QuizStatistic quizStatistic)
        {
            var passedPercent = quizStatistic.CorrectPercent;
            var timeToSolved = new TimeSpan(1, 14, 18);
            var quizStatisticView = new QuizStatisticView()
            {
                PassedPercent = passedPercent,
                TimeToSolved = timeToSolved,
                QuizResultChartView = new QuizResultChartView
                {
                    CorrectAnswers = quizStatistic.CorrectAnswersCount,
                    FaliedAnswers = quizStatistic.FailedAnswersCount,
                    TotalAnswers = quizStatistic.TotalAnswersCount
                }
            };
            return quizStatisticView;
        }
    }
}
