using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary.Specifications
{
    public class SubscriptionsByDateRangeSpec : Specification<Subscription>
    {
        public SubscriptionsByDateRangeSpec(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            if (fromDate.HasValue)
            {
                Query.Where(s => s.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                Query.Where(s => s.CreatedAt <= toDate.Value);
            }

            Query.AsNoTracking();
        }
    }
}
