using Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Statistic.Application.Views
{
    public class UserStatisticByQuizView
    {
        public decimal ScoreForQuiz { get; set; }

        public string TimeToSolved { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public QuizResultChartView QuizResultChartView { get; set; }
    }
}
