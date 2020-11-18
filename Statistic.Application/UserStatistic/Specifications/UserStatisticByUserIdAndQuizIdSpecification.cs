using Shared.Persistence.MongoDb;
using System;

namespace Statistic.Application.UserStatistic.Specifications
{
    using Domain;
    using System.Linq.Expressions;

    public class UserStatisticByUserIdAndQuizIdSpecification : ISpecification<UserStatistic>
    {
        public Expression<Func<UserStatistic, bool>> Predicate { get; }
        public UserStatisticByUserIdAndQuizIdSpecification(Guid userId, Guid quizId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (quizId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(quizId));
            }

            Predicate = d => d.UserId == userId && d.QuizId == quizId;
        }
    }
}
