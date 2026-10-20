using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Subscriptions.Commands.UnfreezeSubscription
{

    [Authorize]
    public class UnfreezeSubscriptionCommand : ICommand, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
    }
}
