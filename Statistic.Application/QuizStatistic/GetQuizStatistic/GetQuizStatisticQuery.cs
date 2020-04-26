using MediatR;
using System;

namespace Statistic.Application.QuizStatistic.GetQuizStatistic
{
    public class GetQuizStatisticQuery : IRequest<Domain.QuizStatistic>
    {
        public Guid QuizId { get; set; }
    }
}
