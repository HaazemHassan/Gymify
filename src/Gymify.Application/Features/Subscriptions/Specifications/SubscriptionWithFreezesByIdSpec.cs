using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Subscriptions.Specifications
{
    public class SubscriptionWithFreezesByIdSpec : Specification<Subscription>, ISingleResultSpecification<Subscription>
    {
        public SubscriptionWithFreezesByIdSpec(int subscriptionId)
        {
            Query.Where(s => s.Id == subscriptionId)
                .Include(s => s.Freezes);
        }
    }
}
