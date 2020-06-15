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
        public decimal CorrectPercent { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
