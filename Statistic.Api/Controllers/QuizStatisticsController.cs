﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;
using Statistic.Application.QuizStatistic.GetQuizStatistic;
using Statistic.Application.Statistic.GetUserStatistic;
using Statistic.Application.Views;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Statistic.Api.Controllers
{
    //[Authorize]
    [Route("api/statistics/quizzes")]
    [ApiController]
    public class QuizStatisticsController : ControllerBase
    {
        private readonly ILogger<QuizStatisticsController> _logger;
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;

        public QuizStatisticsController(ILogger<QuizStatisticsController> logger, 
            IMediator mediator,
            ITracer tracer)
        {
            _logger = logger;
            _mediator = mediator;
            _tracer = tracer;
        }

        [HttpGet]
        [Route("{quizId:guid}")]
        [SwaggerOperation("Get statistic by quiz ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(QuizStatisticView))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Statistic was not found.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Get([FromRoute] GetQuizStatisticQuery query)
        {
            using (var scope = _tracer.BuildSpan("GetQuizStatisticByQuizId").StartActive(finishSpanOnDispose: true))
            {
                var response = await _mediator.Send(query);
                if (response == null)
                {
                    return NotFound($"Statistic for quiz with ID '{query.QuizId}' was not found.");
                }

                //catch if failure
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("{quizId:guid}/users/{userId:guid}")]
        [SwaggerOperation("Get statistic by user ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(QuizStatisticView))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Statistic was not found.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Get([FromRoute] GetUserStatisticByQuizQuery query)
        {
            using (var scope = _tracer.BuildSpan("GetQuizStatisticByUserId").StartActive(finishSpanOnDispose: true))
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