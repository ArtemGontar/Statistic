using MediatR;
using Statistic.Application.QuizStatistic.GetLastQuizUserStatistic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shared.Persistence.MongoDb;
using Statistic.Application.QuizStatistic.Specifications;
using Statistic.Application.Views;
using Statistic.Application.Services;

namespace Statistic.Application.QuizStatistic.GetLastUserQuizStatistic
{
    using Domain;

    public class GetQuizStatisticByUserIdQueryHandler : IRequestHandler<GetQuizStatisticByUserIdQuery, QuizStatisticView>
    {
        private readonly IRepository<QuizStatistic> _quizStatiticRepository;
        private readonly IQuizStatisticService _quizStatisticService;

        public GetQuizStatisticByUserIdQueryHandler(IRepository<QuizStatistic> quizStatiticRepository,
            IQuizStatisticService quizStatisticService)
        {
            _quizStatiticRepository = quizStatiticRepository;
            _quizStatisticService = quizStatisticService;
        }
        public async Task<QuizStatisticView> Handle(GetQuizStatisticByUserIdQuery request, CancellationToken cancellationToken)
        {
            var quizStatisticSpecification = new QuizStatisticByQuizIdAndUserIdSpecification(request.QuizId, request.UserId);
            var quizStatistics = await _quizStatiticRepository.GetAllAsync(quizStatisticSpecification);
            if (quizStatistics == null)
            {
                throw new ArgumentNullException($"Quiz statistic for quiz id {request.QuizId} and user id {request.UserId} not found");
            }

            var lastQuizStatistic = quizStatistics.OrderByDescending(x => x.TimeStamp).FirstOrDefault();

            //TODO: unit testing
            var quizStatisticView = _quizStatisticService.GetQuizStatistic(lastQuizStatistic);
            return quizStatisticView;
        }
    }
}
