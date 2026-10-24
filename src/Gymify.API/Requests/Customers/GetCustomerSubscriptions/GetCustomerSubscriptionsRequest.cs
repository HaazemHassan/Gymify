using Gymify.Api.Requests.Common.Pagination;
using Gymify.Domain.Common.Enums;

namespace Gymify.API.Requests.Customers.GetCustomerSubscriptions
{
    public class GetCustomerSubscriptionsRequest : BasicPaginationRequest
    {
        public SubscriptionStatus? Status { get; set; }
        public SubscriptionType? Type { get; set; }
    }
}
