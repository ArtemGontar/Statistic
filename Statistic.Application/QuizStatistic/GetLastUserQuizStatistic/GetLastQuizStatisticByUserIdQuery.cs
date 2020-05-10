using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Statistic.Application.QuizStatistic.GetLastQuizUserStatistic
{
    using Domain;
    public class GetLastQuizStatisticByUserIdQuery : IRequest<QuizStatistic>
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
    }
}
