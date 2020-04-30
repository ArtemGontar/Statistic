using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Statistic.Application.QuizStatistic.GetQuizStatistic;
using Statistic.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Statistic.Api.Controllers
{
    [Route("api/statistics/quizzes")]
    [ApiController]
    public class QuizStatisticsController : ControllerBase
    {
        private readonly ILogger<QuizStatisticsController> _logger;

        private readonly IMediator _mediator;

        public QuizStatisticsController(ILogger<QuizStatisticsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{quizId:guid}")]
        [SwaggerOperation("Get statistic by quiz ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(QuizStatistic))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Statistic was not found.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Get([FromQuery] GetQuizStatisticQuery query)
        {
            var response = await _mediator.Send(query);
            if (response == null)
            {
                return NotFound($"Statistic for quiz with ID '{response.QuizId}' was not found.");
            }

            //catch if failure
            return Ok(response);
        }
    }
}