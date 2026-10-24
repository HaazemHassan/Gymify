using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Queries.GetCustomerSubscriptions
{
    [Authorize(Permissions = [Permission.SubscriptionsRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfCustomerOnly)]
    public class GetCustomerSubscriptionsQuery : PaginatedQuery, IQuery<PaginatedResult<GetCustomerSubscriptionsQueryResponse>>, IAuthorizedRequest
    {
        public int CustomerId { get; set; }
        public SubscriptionStatus? Status { get; set; }
        public SubscriptionType? Type { get; set; }
    }
}
