using MediatR;
using Shared.Persistence.MongoDb;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Statistic.Application.Statistic.GetUserStatistic
{
    using Domain;
    using Application.UserStatistic.Specifications;

    public class GetLastUserStatisticByQuizIdQueryHandler : IRequestHandler<GetLastUserStatisticByQuizIdQuery, UserStatistic>
    {
        private readonly IRepository<UserStatistic> _userStatiticRepository;

        public GetLastUserStatisticByQuizIdQueryHandler(IRepository<UserStatistic> userStatiticRepository)
        {
            _userStatiticRepository = userStatiticRepository;
        }

        public async Task<UserStatistic> Handle(GetLastUserStatisticByQuizIdQuery request, CancellationToken cancellationToken)
        {
            var userStatisticSpecification = new UserStatisticByUserIdAndQuizIdSpecification(request.UserId, request.QuizId);
            var userStatistics = await _userStatiticRepository.GetAllAsync(userStatisticSpecification);
            if(userStatistics == null)
            {
                throw new ArgumentNullException($"User statistic for user id {request.UserId} and quiz id {request.QuizId} not found");
            }

            var lastUserStatistic = userStatistics.OrderByDescending(x => x.TimeStamp).FirstOrDefault();
            return lastUserStatistic;
        }
    }
}
