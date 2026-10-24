using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Commands.UnfreezeSubscription
{
    [Authorize(Permissions = [Permission.SubscriptionsWrite])]
    public class UnfreezeSubscriptionCommand : ICommand<UnfreezeSubscriptionCommandResponse>, IAuthorizedRequest
    {
        public UnfreezeSubscriptionCommand(int subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }

        public int SubscriptionId { get; set; }
    }
}
