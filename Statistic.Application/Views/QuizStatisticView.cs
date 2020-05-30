using System;

namespace Statistic.Application.Views
{
    public class QuizStatisticView
    {
        public double PassedPercent { get; set; }
        public TimeSpan TimeToSolved { get; set; }

        public LastScoresChartView LastScoresChartView { get; set; } = new LastScoresChartView();
        public QuizResultChartView QuizResultChartView { get; set; } = new QuizResultChartView();
    }
}
