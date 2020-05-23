using Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Statistic.Application.Views
{
    public class UserStatisticView
    {
        public int TotalPassedQuizzes { get; set; }

        public double ScoreForQuizzes { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public int TotalPassedQuestions { get; set; }

        public int TotalFailedQuestions { get; set; }

        public double PassedPercent { get; set; }

        public LastScoresChartView LastScoresChartView { get; set; }

        public QuizResultChartView QuizResultChartView { get; set; }
    }
}
