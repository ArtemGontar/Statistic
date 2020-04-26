using MongoDB.Driver;
using Shared.Persistence.MongoDb;
using Statistic.Domain;
using System;
using System.Threading.Tasks;

namespace Statistic.Persistence
{
    public class QuizStatisticRepository : Repository<QuizStatistic>
    {
        public QuizStatisticRepository(StatisticDbContext context) : base(context)
        {
            
        }

        public override async Task<bool> SaveAsync(QuizStatistic entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            var update = Update
                .Set(x => x.Score, entity.Score)
                .Set(x => x.QuizId, entity.QuizId);

            var result = await Collection.UpdateOneAsync(FilterId(entity.Id), update, OptionUpsert);

            return IsUpdated(result);
        }

        public override async Task<bool> DeleteAsync(ISpecification<QuizStatistic> specification)
        {
            var result = await Collection.DeleteOneAsync(specification.Predicate);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
