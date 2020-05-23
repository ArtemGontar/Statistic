using MediatR;
using System;

namespace Statistic.Application.QuizStatistic.GetQuizStatistic
{
    using Domain;
    using global::Statistic.Application.Views;
    using System.Collections.Generic;

    public class GetQuizStatisticQuery : IRequest<QuizStatisticView>
    {
        public Guid QuizId { get; set; }
    }
}
