using Shared.Common;

namespace Statistic.Application.Views
{
    public class UserStatisticView
    {
        public int TotalPassedQuizzes { get; set; }

        public decimal ScoreForQuizzes { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public int TotalPassedQuestions { get; set; }

        public int TotalFailedQuestions { get; set; }

        public decimal PassedPercent { get; set; }

        public LastScoresChartView LastScoresChartView { get; set; } = new LastScoresChartView();

        public QuizResultChartView QuizResultChartView { get; set; } = new QuizResultChartView();
    }
}
