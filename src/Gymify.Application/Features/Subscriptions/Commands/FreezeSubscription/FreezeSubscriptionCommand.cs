using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Commands.FreezeSubscription
{
    [Authorize(Permissions = [Permission.SubscriptionsWrite])]
    public class FreezeSubscriptionCommand : ICommand<FreezeSubscriptionCommandResponse>, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
