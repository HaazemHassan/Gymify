using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Subscriptions.Commands.CancelSubscription
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class CancelSubscriptionCommand : ICommand, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
    }
}
