using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Subscriptions.Commands.FreezeSubscription
{

    [Authorize]
    public class FreezeSubscriptionCommand : ICommand, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
    }
}
