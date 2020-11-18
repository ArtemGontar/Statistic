using System;
using System.Collections.Generic;
using System.Text;

namespace Statistic.Application.Views
{
    /// <summary>
    /// Show last passed quizzes in axis-X and scores axis-Y
    /// </summary>
    public class LastScoresChartView
    {
        public IEnumerable<QuizScoreView> QuizScores { get; set; }
    }
}
