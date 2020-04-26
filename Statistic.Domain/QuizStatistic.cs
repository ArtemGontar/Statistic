using System;

namespace Statistic.Domain
{
    public class QuizStatistic
    {
        public Guid Id { get; set; }
        public string Score { get; set; }
        public Guid QuizId { get; set; }
    }
}
