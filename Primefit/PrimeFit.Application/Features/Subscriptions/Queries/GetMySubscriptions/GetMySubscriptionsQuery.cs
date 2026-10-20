using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions
{
    [Authorize]
    public class GetMySubscriptionsQuery : PaginatedQuery, IQuery<PaginatedResult<GetMySubscriptionsQueryResponse>>, IAuthorizedRequest
    {
        public SubscriptionStatus? Status { get; set; }
    }
}
