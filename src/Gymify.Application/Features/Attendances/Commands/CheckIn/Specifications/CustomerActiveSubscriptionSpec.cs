using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn.Specifications
{
    public class CustomerActiveSubscriptionSpec : Specification<Subscription>, ISingleResultSpecification<Subscription>
    {
        public CustomerActiveSubscriptionSpec(int customerId, DateTimeOffset now)
        {
            Query.Where(s => s.CustomerId == customerId
                          && !s.IsCancelled
                          && s.StartDate <= now
                          && s.EndDate >= now
                          && !s.Freezes.Any(f => f.StartDate <= now && f.EndDate >= now))
                .Include(s => s.Freezes)
                .OrderByDescending(s => s.EndDate);
        }
    }
}
