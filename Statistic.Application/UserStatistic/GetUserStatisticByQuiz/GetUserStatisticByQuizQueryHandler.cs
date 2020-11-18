using MediatR;
using Shared.Persistence.MongoDb;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using Statistic.Application.Views;
using Statistic.Application.Services;
using Statistic.Application.UserStatistic.Specifications;
using Shared.Common;

namespace Statistic.Application.Statistic.GetUserStatistic
{
    using Domain;
    
    public class GetUserStatisticByQuizQueryHandler : IRequestHandler<GetUserStatisticByQuizQuery, UserStatisticByQuizView>
    {
        private readonly IRepository<UserStatistic> _userStatiticRepository;
        private readonly IUserStatisticService _userStatisticService;

        public GetUserStatisticByQuizQueryHandler(IRepository<UserStatistic> userStatiticRepository,
            IUserStatisticService userStatisticService)
        {
            _userStatiticRepository = userStatiticRepository;
            _userStatisticService = userStatisticService;
        }

        public async Task<UserStatisticByQuizView> Handle(GetUserStatisticByQuizQuery request, CancellationToken cancellationToken)
        {
            var userStatisticSpecification = new UserStatisticByUserIdAndQuizIdSpecification(request.UserId, request.QuizId);
            var userStatistics = await _userStatiticRepository.GetAllAsync(userStatisticSpecification);
            if(userStatistics == null)
            {
                throw new ArgumentNullException($"User statistic for user id {request.UserId} and quiz id {request.QuizId} not found");
            }

            var lastUserStatistic = userStatistics.OrderByDescending(x => x.TimeStamp).FirstOrDefault();

            var userStatisticByQiuzView = _userStatisticService.GetUserStatistic(lastUserStatistic, EnglishLevel.Beginner);
            return userStatisticByQiuzView;
        }
    }
}
