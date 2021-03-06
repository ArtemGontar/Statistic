﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;
using Statistic.Application.Statistic.GetUserStatistic;
using Statistic.Application.UserStatistic.GetUserStatistic;
using Statistic.Domain;
using Swashbuckle.AspNetCore.Annotations;

namespace Statistic.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/statistics/users")]
    public class UserStatisticsController : ControllerBase
    {
        private readonly ILogger<UserStatisticsController> _logger;
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;

        public UserStatisticsController(ILogger<UserStatisticsController> logger, 
            IMediator mediator,
            ITracer tracer)
        {
            _logger = logger;
            _mediator = mediator;
            _tracer = tracer;
        }

        [HttpGet]
        [Route("{userId:guid}")]
        [SwaggerOperation("Get statistic by user ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(IEnumerable<UserStatistic>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Statistic was not found.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Get([FromRoute] GetUserStatisticQuery query)
        {
            using (var scope = _tracer.BuildSpan("GetUserStatisticByUserId").StartActive(finishSpanOnDispose: true))
            {
                var response = await _mediator.Send(query);
                if (response == null)
                {
                    return NotFound($"Statistic for user with ID '{query.UserId}' was not found.");
                }

                //catch if failure
                return Ok(response);
            }
            
        }

        [HttpGet]
        [Route("{userId:guid}/quizzes/{quizId:guid}")]
        [SwaggerOperation("Get statistic by user ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(IEnumerable<UserStatistic>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Statistic was not found.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Get([FromRoute] GetUserStatisticByQuizQuery query)
        {
            using (var scope = _tracer.BuildSpan("GetUserStatisticByQuizId").StartActive(finishSpanOnDispose: true))
            {
                var response = await _mediator.Send(query);
                if (response == null)
                {
                    return NotFound($"Statistic for user with ID '{query.UserId}' and quiz id {query.QuizId} was not found.");
                }

                //catch if failure
                return Ok(response);
            }
        }
    }
}
