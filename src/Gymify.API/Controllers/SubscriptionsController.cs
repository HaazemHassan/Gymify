using AutoMapper;
using Gymify.API.Common.Constants;
using Gymify.API.Requests.Subscriptions.CancelSubscription;
using Gymify.API.Requests.Subscriptions.CreateSubscription;
using Gymify.API.Requests.Subscriptions.FreezeSubscription;
using Gymify.API.Requests.Subscriptions.GetFinancialSummary;
using Gymify.API.Requests.Subscriptions.GetSubscriptionsForManagement;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Features.Subscriptions.Commands.CancelSubscription;
using Gymify.Application.Features.Subscriptions.Commands.CreateSubscription;
using Gymify.Application.Features.Subscriptions.Commands.FreezeSubscription;
using Gymify.Application.Features.Subscriptions.Commands.UnfreezeSubscription;
using Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary;
using Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionById;
using Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionsForManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// Retrieves all subscriptions for management with pagination, search, status filter, and type filter. Requires SubscriptionsRead permission.
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(typeof(PaginatedResult<GetSubscriptionsForManagementQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSubscriptionsForManagement([FromQuery] GetSubscriptionsForManagementRequest request, CancellationToken ct)
        {
            var query = _mapper.Map<GetSubscriptionsForManagementQuery>(request);
            var result = await Mediator.Send(query, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves subscription details by ID along with its freeze history. Requires SubscriptionsRead permission.
        /// </summary>
        [HttpGet("{id:int}", Name = RouteNames.Subscriptions.GetSubscriptionById)]
        [ProducesResponseType(typeof(GetSubscriptionByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscriptionById([FromRoute] int id, CancellationToken ct)
        {
            var result = await Mediator.Send(new GetSubscriptionByIdQuery(id), ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Creates a new subscription for a customer with package details and payment method. Requires SubscriptionsWrite permission.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreateSubscriptionCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
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
        /// Freezes a subscription for a specified date range and extends the subscription end date accordingly. Requires SubscriptionsWrite permission.
        /// </summary>
        [HttpPost("{id:int}/freeze")]
        [ProducesResponseType(typeof(FreezeSubscriptionCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> FreezeSubscription([FromRoute] int id, [FromBody] FreezeSubscriptionRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<FreezeSubscriptionCommand>(request);
            command.SubscriptionId = id;

            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Unfreezes an active subscription early and readjusts the subscription end date based on actual freeze duration. Requires SubscriptionsWrite permission.
        /// </summary>
        [HttpPost("{id:int}/unfreeze")]
        [ProducesResponseType(typeof(UnfreezeSubscriptionCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UnfreezeSubscription([FromRoute] int id, CancellationToken ct)
        {
            var command = new UnfreezeSubscriptionCommand(id);
            var result = await Mediator.Send(command, ct);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Cancels an active or upcoming subscription with an optional cancellation reason. Requires SubscriptionsWrite permission.
        /// </summary>
        [HttpPost("{id:int}/cancel")]
        [ProducesResponseType(typeof(CancelSubscriptionCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CancelSubscription([FromRoute] int id, [FromBody] CancelSubscriptionRequest request, CancellationToken ct)
        {
            var command = _mapper.Map<CancelSubscriptionCommand>(request);
            command.SubscriptionId = id;

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
