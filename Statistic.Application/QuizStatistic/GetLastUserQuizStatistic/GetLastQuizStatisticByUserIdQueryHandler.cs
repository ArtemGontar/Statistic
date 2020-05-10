using MediatR;
using Statistic.Application.QuizStatistic.GetLastQuizUserStatistic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shared.Persistence.MongoDb;

namespace Statistic.Application.QuizStatistic.GetLastUserQuizStatistic
{
    using Application.QuizStatistic.Specifications;
    using Domain;

    public class GetLastQuizStatisticByUserIdQueryHandler : IRequestHandler<GetLastQuizStatisticByUserIdQuery, QuizStatistic>
    {
        private readonly IRepository<QuizStatistic> _quizStatiticRepository;

        public GetLastQuizStatisticByUserIdQueryHandler(IRepository<QuizStatistic> quizStatiticRepository)
        {
            _quizStatiticRepository = quizStatiticRepository;
        }
        public async Task<QuizStatistic> Handle(GetLastQuizStatisticByUserIdQuery request, CancellationToken cancellationToken)
        {
            var quizStatisticSpecification = new QuizStatisticByQuizIdAndUserIdSpecification(request.QuizId, request.UserId);
            var quizStatistics = await _quizStatiticRepository.GetAllAsync(quizStatisticSpecification);
            if (quizStatistics == null)
            {
                throw new ArgumentNullException($"Quiz statistic for quiz id {request.QuizId} and user id {request.UserId} not found");
            }

            var lastQuizStatistic = quizStatistics.OrderByDescending(x => x.TimeStamp).FirstOrDefault();
            return lastQuizStatistic;
        }
    }
}
