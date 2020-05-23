namespace Statistic.Application.Views
{
    /// <summary>
    /// Show total passed questions and correct/Falied answers
    /// </summary>
    public class QuizResultChartView
    {
        public int CorrectAnswers { get; set; }
        public int FaliedAnswers { get; set; }
        public int TotalAnswers { get; set; }
    }
}
