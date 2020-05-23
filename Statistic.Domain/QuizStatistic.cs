using System;

namespace Statistic.Domain
{
    public class QuizStatistic
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public int TotalAnswersCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int FailedAnswersCount { get; set; }
        public double CorrectPercent { get; set; }
        public Guid UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
