using MediatR;

namespace Statistic.Application.UserStatistic.GetUserStatistic
{
    using Domain;
    using System;
    using System.Collections.Generic;

    public class GetUserStatisticQuery : IRequest<IEnumerable<UserStatistic>>
    {
        public Guid UserId { get; set; }
    }
}
