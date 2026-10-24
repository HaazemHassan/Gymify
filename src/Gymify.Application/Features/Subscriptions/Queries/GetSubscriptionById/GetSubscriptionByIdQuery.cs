using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionById
{
    [Authorize(Permissions = [Permission.SubscriptionsRead])]
    [Authorize(Policy = AuthorizationPolicies.SelfSubscriptionOnly)]
    public class GetSubscriptionByIdQuery : IQuery<GetSubscriptionByIdQueryResponse>, ISubscriptionResourceRequest
    {
        public GetSubscriptionByIdQuery()
        {
        }

        public GetSubscriptionByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public int SubscriptionId => Id;
    }
}
