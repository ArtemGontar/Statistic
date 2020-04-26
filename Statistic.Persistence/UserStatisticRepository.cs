using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Shared.Persistence.MongoDb;
using Statistic.Domain;

namespace Statistic.Persistence
{
    public class UserStatisticRepository : Repository<UserStatistic>
    {
        public UserStatisticRepository(StatisticDbContext context) : base(context)
        {
            
        }

        public override async Task<bool> SaveAsync(UserStatistic entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            var update = Update
                .Set(x => x.Score, entity.Score)
                .Set(x => x.UserId, entity.UserId);

            var result = await Collection.UpdateOneAsync(FilterId(entity.Id), update, OptionUpsert);

            return IsUpdated(result);
        }

        public override async Task<bool> DeleteAsync(ISpecification<UserStatistic> specification)
        {
            var result = await Collection.DeleteOneAsync(specification.Predicate);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
