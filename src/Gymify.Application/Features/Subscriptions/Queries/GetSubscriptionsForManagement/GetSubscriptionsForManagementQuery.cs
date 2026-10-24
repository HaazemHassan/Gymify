using Gymify.Application.Common.CQRS;
using Gymify.Application.Common.Pagination;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionsForManagement
{
    [Authorize(Permissions = [Permission.SubscriptionsRead])]
    public class GetSubscriptionsForManagementQuery : PaginatedQuery, IQuery<PaginatedResult<GetSubscriptionsForManagementQueryResponse>>, IAuthorizedRequest
    {
        public SubscriptionStatus? Status { get; set; }
        public SubscriptionType? Type { get; set; }
    }
}
