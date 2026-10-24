using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn.Specifications
{
    public class LatestSubscriptionByCustomerIdSpec : Specification<Subscription>, ISingleResultSpecification<Subscription>
    {
        public LatestSubscriptionByCustomerIdSpec(int customerId)
        {
            Query.Where(s => s.CustomerId == customerId)
                .Include(s => s.Freezes)
                .OrderByDescending(s => s.EndDate);
        }
    }
}
