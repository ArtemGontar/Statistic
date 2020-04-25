using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Statistic.Application.Statistic.GetUserStatistic;
using Statistic.Domain;
using Swashbuckle.AspNetCore.Annotations;

namespace Statistic.Api.Controllers
{
    [ApiController]
    [Route("statistic")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;

        private readonly IMediator _mediator;

        public StatisticsController(ILogger<StatisticsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{userId:guid}")]
        [SwaggerOperation("Get quiz by ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(UserStatistic))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Quiz was not found.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Get([FromQuery] GetUserStatisticQuery query)
        {
            var response = await _mediator.Send(new GetUserStatisticQuery());
            if (response == null)
            {
                return NotFound($"Statistic for user with ID '{response.UserId}' was not found.");
            }

            //catch if failure
            return Ok(response);
        }
    }
}
