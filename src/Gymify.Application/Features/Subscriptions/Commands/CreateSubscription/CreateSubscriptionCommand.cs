using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Commands.CreateSubscription
{
    [Authorize(Permissions = [Permission.SubscriptionsWrite])]
    public class CreateSubscriptionCommand : ICommand<CreateSubscriptionCommandResponse>, IAuthorizedRequest
    {
        public int CustomerId { get; set; }
        public int PackageId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Notes { get; set; }
    }
}
