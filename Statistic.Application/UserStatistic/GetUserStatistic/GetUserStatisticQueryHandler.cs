using MediatR;
using Statistic.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Statistic.Application.Statistic.GetUserStatistic
{
    public class GetUserStatisticQueryHandler : IRequestHandler<GetUserStatisticQuery, UserStatistic>
    {
        public GetUserStatisticQueryHandler()
        {
            
        }

        public Task<UserStatistic> Handle(GetUserStatisticQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new UserStatistic());
        }
    }
}
