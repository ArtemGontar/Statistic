using MongoDB.Driver;
using Shared.Persistence.MongoDb;
using Statistic.Domain;
using System;
using System.Threading.Tasks;

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
                .Set(x => x.UserId, entity.UserId)
                .Set(x => x.QuestionsCount, entity.QuestionsCount)
                .Set(x => x.CorrectAnswersCount, entity.CorrectAnswersCount)
                .Set(x => x.FailedAnswersCount, entity.FailedAnswersCount)
                .Set(x => x.CorrectPercent, entity.CorrectPercent)
                .Set(x => x.TimeStamp, entity.TimeStamp)
                .Set(x => x.QuizId, entity.QuizId);


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
