using System;
using Microsoft.Extensions.Options;
using Shared.Persistence.MongoDb;
using Statistic.Domain;

namespace Statistic.Persistence
{
    public class StatisticDbContext : DbContext
    {
        public StatisticDbContext(IOptions<ConnectionStrings> connectionStrings): base(connectionStrings)
        {   
        }

        protected override void RegisterClassMaps()
        {
            RegisterClassMap<UserStatistic, Guid>(d => d.Id);
            RegisterClassMap<QuizStatistic, Guid>(d => d.Id);
        }
    }
}
