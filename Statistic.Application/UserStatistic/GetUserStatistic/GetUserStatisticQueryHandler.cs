using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Persistence.MongoDb;

namespace Statistic.Application.UserStatistic.GetUserStatistic
{
    using Domain;
    using Application.UserStatistic.Specifications;

    public class GetUserStatisticQueryHandler : IRequestHandler<GetUserStatisticQuery, IEnumerable<UserStatistic>>
    {
        private readonly IRepository<UserStatistic> _userStatiticRepository;

        public GetUserStatisticQueryHandler(IRepository<UserStatistic> userStatiticRepository)
        {
            _userStatiticRepository = userStatiticRepository;
        }

        public async Task<IEnumerable<UserStatistic>> Handle(GetUserStatisticQuery request, CancellationToken cancellationToken)
        {
            var userStatisticSpecification = new UserStatisticByUserIdSpecification(request.UserId);
            var userStatistics = await _userStatiticRepository.GetAllAsync(userStatisticSpecification);
            if (userStatistics == null)
            {
                throw new ArgumentNullException($"User statistic for user {request.UserId} not found");
            }
            return userStatistics;
        }
    }
}
