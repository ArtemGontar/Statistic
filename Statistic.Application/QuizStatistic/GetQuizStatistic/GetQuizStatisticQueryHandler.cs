using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Statistic.Application.QuizStatistic.GetQuizStatistic
{
    using Domain;
    using global::Statistic.Application.QuizStatistic.Specifications;
    using Shared.Persistence.MongoDb;
    using System;
    using System.Collections.Generic;

    public class GetQuizStatisticQueryHandler : IRequestHandler<GetQuizStatisticQuery, IEnumerable<QuizStatistic>>
    {
        private readonly IRepository<QuizStatistic> _quizStatiticRepository;
        public GetQuizStatisticQueryHandler(IRepository<QuizStatistic> quizStatiticRepository)
        {
            _quizStatiticRepository = quizStatiticRepository;
        }

        public async Task<IEnumerable<QuizStatistic>> Handle(GetQuizStatisticQuery request, CancellationToken cancellationToken)
        {
            var quizStatisticSpecification = new QuizStatisticByQuizIdSpecification(request.QuizId);
            var quizStatistics = await _quizStatiticRepository.GetAllAsync(quizStatisticSpecification);
            if (quizStatistics == null)
            {
                throw new ArgumentNullException($"Quiz statistic for quiz {request.QuizId} not found");
            }
            return quizStatistics;
        }
    }
}
