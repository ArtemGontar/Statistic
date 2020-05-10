using System;

namespace Statistic.Domain
{
    public class QuizStatistic
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public int QuestionsCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int WrongAnswersCount { get; set; }
        public double CorrectPercent { get; set; }
        public Guid UserId { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
