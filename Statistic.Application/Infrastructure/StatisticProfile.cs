using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Shared.Bus.Messages;


namespace Statistic.Application.Infrastructure
{
    using Domain;
    public class StatisticProfile : Profile
    {
        public StatisticProfile()
        {
            CreateMap<QuizResultMessage, QuizStatistic>();

            CreateMap<QuizResultMessage, UserStatistic>();
        }
    }
}
