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
    [Route("api/statistics/users")]
    public class UserStatisticsController : ControllerBase
    {
        private readonly ILogger<UserStatisticsController> _logger;

        private readonly IMediator _mediator;

        public UserStatisticsController(ILogger<UserStatisticsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{userId:guid}")]
        [SwaggerOperation("Get statistic by user ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(UserStatistic))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Statistic was not found.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Get([FromQuery] GetUserStatisticQuery query)
        {
            var response = await _mediator.Send(query);
            if (response == null)
            {
                return NotFound($"Statistic for user with ID '{response.UserId}' was not found.");
            }

            //catch if failure
            return Ok(response);
        }
    }
}
