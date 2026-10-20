using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gymify.API.Requests.Subscriptions.CreateSubscription;
using Gymify.Application.Features.Subscriptions.Commands.CreateSubscription;

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
    }
}
