using MassTransit;
using System;
using System.Threading.Tasks;
using Shared.Bus.Messages;
using Shared.Persistence.MongoDb;

namespace Statistic.Application.Consumers
{
    using AutoMapper;
    using Domain;
    public class QuizResultConsumer :
        IConsumer<QuizResultMessage>
    {
        private readonly IRepository<QuizStatistic> _quizStatisticRepository;
        private readonly IRepository<UserStatistic> _userStatisticRepository;
        private readonly IMapper _mapper;
        public QuizResultConsumer(IRepository<QuizStatistic> quizStatisticRepository,
            IRepository<UserStatistic> userStatisticRepository,
            IMapper mapper
            )
        {
            _quizStatisticRepository = quizStatisticRepository;
            _userStatisticRepository = userStatisticRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<QuizResultMessage> context)
        {
            var message = context.Message;
            var quizStatistic = _mapper.Map<QuizStatistic>(message);
            if(!await _quizStatisticRepository.SaveAsync(quizStatistic))
            {
                throw new InvalidOperationException($"Quiz statistic save failed");
            }

            var userStatistic = _mapper.Map<UserStatistic>(message);
            if (!await _userStatisticRepository.SaveAsync(userStatistic))
            {
                throw new InvalidOperationException($"User statistic save failed");
            }
        }
    }
}
