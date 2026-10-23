using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.API.Requests.Subscriptions.CreateSubscription;
using Gymify.API.Requests.Subscriptions.GetFinancialSummary;
using Gymify.Application.Features.Subscriptions.Commands.CreateSubscription;
using Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary;

namespace Gymify.API.Controllers
{
    [Authorize]
    public class SubscriptionsController : BaseController
    {
        private readonly IMapper _mapper;

        public SubscriptionsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new subscription for a customer with package details and payment method. Requires SubscriptionsWrite permission.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CreateSubscriptionCommand>(request);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves total gym revenue and a breakdown by payment method for a specified date range. Requires SubscriptionsRead permission.
        /// </summary>
        [HttpGet("financial-summary")]
        [ProducesResponseType(typeof(GetFinancialSummaryQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFinancialSummary([FromQuery] GetFinancialSummaryRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetFinancialSummaryQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
