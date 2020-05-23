using MediatR;
using Statistic.Application.Views;
using System;

namespace Statistic.Application.UserStatistic.GetUserStatistic
{

    public class GetUserStatisticQuery : IRequest<UserStatisticView>
    {
        public Guid UserId { get; set; }
    }
}
