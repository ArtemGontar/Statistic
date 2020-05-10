using MediatR;
using System;

namespace Statistic.Application.QuizStatistic.GetQuizStatistic
{
    using Domain;
    using System.Collections.Generic;

    public class GetQuizStatisticQuery : IRequest<IEnumerable<QuizStatistic>>
    {
        public Guid QuizId { get; set; }
    }
}
