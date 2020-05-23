using System;
using MediatR;
using Statistic.Application.Views;

namespace Statistic.Application.Statistic.GetUserStatistic
{
    
    public class GetUserStatisticByQuizQuery : IRequest<UserStatisticByQuizView>
    {
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
    }
}
