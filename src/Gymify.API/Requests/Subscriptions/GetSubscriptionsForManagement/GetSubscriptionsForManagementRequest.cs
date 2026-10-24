using Gymify.Api.Requests.Common.Pagination;
using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Subscriptions.GetSubscriptionsForManagement
{
    public class GetSubscriptionsForManagementRequest : BasicPaginationRequest
    {
        public SubscriptionStatus? Status { get; set; }
        public SubscriptionType? Type { get; set; }
    }
}
