using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Statistic.Application.QuizStatistic.GetLastQuizUserStatistic
{
    using Domain;
    using global::Statistic.Application.Views;

    public class GetQuizStatisticByUserIdQuery : IRequest<QuizStatisticView>
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
    }
}
