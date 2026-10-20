using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Subscriptions.Commands.CreateSubscription.Specifications
{
    public class ActiveSubscriptionByCustomerIdSpec : Specification<Subscription>
    {
        public ActiveSubscriptionByCustomerIdSpec(int customerId, DateTimeOffset now)
        {
            Query.Where(s => s.CustomerId == customerId && s.EndDate >= now);
        }
    }
}
