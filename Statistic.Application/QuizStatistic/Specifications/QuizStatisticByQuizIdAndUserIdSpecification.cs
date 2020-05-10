using Shared.Persistence.MongoDb;
using System;

namespace Statistic.Application.QuizStatistic.Specifications
{
    using Domain;
    using System.Linq.Expressions;

    public class QuizStatisticByQuizIdAndUserIdSpecification : ISpecification<QuizStatistic>
    {
        public Expression<Func<QuizStatistic, bool>> Predicate { get; }
        public QuizStatisticByQuizIdAndUserIdSpecification(Guid quizId, Guid userId)
        {
            if (quizId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(quizId));
            }

            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Predicate = d =>  d.QuizId == quizId && d.UserId == userId;
        }
    }
}
