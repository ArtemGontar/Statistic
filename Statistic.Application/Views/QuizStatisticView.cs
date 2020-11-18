using System;

namespace Statistic.Application.Views
{
    public class QuizStatisticView
    {
        public decimal PassedPercent { get; set; }
        public string TimeToSolved { get; set; }

        public LastScoresChartView LastScoresChartView { get; set; } = new LastScoresChartView();
        public QuizResultChartView QuizResultChartView { get; set; } = new QuizResultChartView();
    }
}
