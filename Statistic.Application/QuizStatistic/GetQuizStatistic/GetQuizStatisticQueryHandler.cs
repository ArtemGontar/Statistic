using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Statistic.Application.QuizStatistic.GetQuizStatistic
{
    using Domain;
    public class GetQuizStatisticQueryHandler : IRequestHandler<GetQuizStatisticQuery, QuizStatistic>
    {
        public GetQuizStatisticQueryHandler()
        {

        }

        public Task<QuizStatistic> Handle(GetQuizStatisticQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Domain.QuizStatistic());
        }
    }
}
