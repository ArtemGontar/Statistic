using Shared.Persistence.MongoDb;
using System;
using System.Linq.Expressions;

namespace Statistic.Application.UserStatistic.Specifications
{
    using Domain;
    public class UserStatisticByUserIdSpecification : ISpecification<UserStatistic>
    {
        public Expression<Func<UserStatistic, bool>> Predicate { get; }
        public UserStatisticByUserIdSpecification(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Predicate = d => d.UserId == userId;
        }
    }
}
