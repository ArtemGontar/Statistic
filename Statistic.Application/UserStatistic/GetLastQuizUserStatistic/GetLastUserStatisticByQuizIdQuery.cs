using System;
using MediatR;

namespace Statistic.Application.Statistic.GetUserStatistic
{
    using Domain;
    public class GetLastUserStatisticByQuizIdQuery : IRequest<UserStatistic>
    {
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
    }
}
