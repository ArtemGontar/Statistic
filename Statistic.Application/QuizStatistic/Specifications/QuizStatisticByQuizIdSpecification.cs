using Shared.Persistence.MongoDb;
using System;

namespace Statistic.Application.QuizStatistic.Specifications
{
    using Domain;
    using System.Linq.Expressions;

    public class QuizStatisticByQuizIdSpecification : ISpecification<QuizStatistic>
    {
        public Expression<Func<QuizStatistic, bool>> Predicate { get; }
        public QuizStatisticByQuizIdSpecification(Guid quizId)
        {
            if (quizId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(quizId));
            }

            Predicate = d => d.QuizId == quizId;
        }
    }
}
