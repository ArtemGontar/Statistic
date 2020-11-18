using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Statistic.Application.QuizStatistic.Specifications;
using Statistic.Application.Views;
using Shared.Persistence.MongoDb;
using Statistic.Application.Services;
using System;

namespace Statistic.Application.QuizStatistic.GetQuizStatistic
{
    using Domain;
    using global::Statistic.Application.Services;


    public class GetQuizStatisticQueryHandler : IRequestHandler<GetQuizStatisticQuery, QuizStatisticView>
    {
        private readonly IRepository<QuizStatistic> _quizStatiticRepository;
        private readonly IQuizStatisticService _quizStatisticService;
        public GetQuizStatisticQueryHandler(IRepository<QuizStatistic> quizStatiticRepository,
            IQuizStatisticService quizStatisticService)
        {
            _quizStatiticRepository = quizStatiticRepository;
            _quizStatisticService = quizStatisticService;
        }

        public async Task<QuizStatisticView> Handle(GetQuizStatisticQuery request, CancellationToken cancellationToken)
        {
            var quizStatisticSpecification = new QuizStatisticByQuizIdSpecification(request.QuizId);
            var quizStatistics = await _quizStatiticRepository.GetAllAsync(quizStatisticSpecification);
            if (quizStatistics == null)
            {
                throw new ArgumentNullException($"Quiz statistic for quiz {request.QuizId} not found");
            }

            //TODO: unit testing
            var quizStatisticView = _quizStatisticService.GetQuizStatistics(quizStatistics);

            return quizStatisticView;
        }
    }
}
