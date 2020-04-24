using System;
using MediatR;
using Statistic.Domain;

namespace Statistic.Application.Statistic.GetUserStatistic
{
    public class GetUserStatisticQuery : IRequest<UserStatistic>
    {
        public Guid UserId { get; set; }
    }
}
