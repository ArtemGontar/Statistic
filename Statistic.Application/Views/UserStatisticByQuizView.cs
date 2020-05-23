using System;
using System.Collections.Generic;
using System.Text;

namespace Statistic.Application.Views
{
    public class UserStatisticByQuizView
    {
        public double ScoreForQuiz { get; set; }

        public TimeSpan TimeToSolved { get; set; }

        public QuizResultChartView QuizResultChartView { get; set; }
    }
}
