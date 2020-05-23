using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Persistence.MongoDb;
using Statistic.Application.UserStatistic.Specifications;
using Statistic.Application.Services;
using Shared.Common;
using Statistic.Application.Views;

namespace Statistic.Application.UserStatistic.GetUserStatistic
{
    using Domain;
    
    public class GetUserStatisticQueryHandler : IRequestHandler<GetUserStatisticQuery, UserStatisticView>
    {
        private readonly IRepository<UserStatistic> _userStatiticRepository;
        private readonly IUserStatisticService _userStatisticService;

        public GetUserStatisticQueryHandler(IRepository<UserStatistic> userStatiticRepository,
            IUserStatisticService userStatisticService)
        {
            _userStatiticRepository = userStatiticRepository;
            _userStatisticService = userStatisticService;
        }

        public async Task<UserStatisticView> Handle(GetUserStatisticQuery request, CancellationToken cancellationToken)
        {
            var userStatisticSpecification = new UserStatisticByUserIdSpecification(request.UserId);
            var userStatistics = await _userStatiticRepository.GetAllAsync(userStatisticSpecification);
            if (userStatistics == null)
            {
                throw new ArgumentNullException($"User statistic for user {request.UserId} not found");
            }

            var userStatisticView = _userStatisticService.GetUserStatistics(userStatistics, EnglishLevel.Beginner);

            return userStatisticView;
        }
    }
}
