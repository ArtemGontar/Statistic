﻿using System;

namespace Statistic.Domain
{
    public class UserStatistic
    {
        public Guid Id { get; set; }
        public string Score { get; set; }
        public Guid UserId { get; set; }
    }
}
