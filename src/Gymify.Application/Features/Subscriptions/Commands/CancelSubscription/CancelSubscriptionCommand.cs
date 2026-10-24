using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Commands.CancelSubscription
{
    [Authorize(Permissions = [Permission.SubscriptionsWrite])]
    public class CancelSubscriptionCommand : ICommand<CancelSubscriptionCommandResponse>, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
        public string? Reason { get; set; }
    }
}
