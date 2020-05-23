using System;

namespace Statistic.Application.Views
{
    public class QuizStatisticView
    {
        public double PassedPercent { get; set; }
        public TimeSpan TimeToSolved { get; set; }
        public QuizResultChartView QuizResultChart { get;set; }
    }
}
