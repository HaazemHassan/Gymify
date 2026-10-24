using Ardalis.Specification;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Subscriptions.Specifications
{
    public class SubscriptionsFilteredSpec : Specification<Subscription>
    {
        public SubscriptionsFilteredSpec(
            DateTimeOffset now,
            SubscriptionStatus? status = null,
            string? search = null,
            SubscriptionType? type = null,
            int? customerId = null)
        {
            ApplyFilters(Query, now, status, search, type, customerId);
        }

        public static void ApplyFilters(
            ISpecificationBuilder<Subscription> builder,
            DateTimeOffset now,
            SubscriptionStatus? status = null,
            string? search = null,
            SubscriptionType? type = null,
            int? customerId = null)
        {
            if (customerId.HasValue)
            {
                builder.Where(s => s.CustomerId == customerId.Value);
            }

            if (type.HasValue)
            {
                builder.Where(s => s.Type == type.Value);
            }

            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case SubscriptionStatus.Active:
                        builder.Where(s => !s.IsCancelled
                                      && s.StartDate <= now
                                      && s.EndDate >= now
                                      && !s.Freezes.Any(f => f.StartDate <= now && f.EndDate >= now));
                        break;

                    case SubscriptionStatus.Frozen:
                        builder.Where(s => !s.IsCancelled
                                      && s.Freezes.Any(f => f.StartDate <= now && f.EndDate >= now));
                        break;

                    case SubscriptionStatus.Expired:
                        builder.Where(s => !s.IsCancelled && s.EndDate < now);
                        break;

                    case SubscriptionStatus.Cancelled:
                        builder.Where(s => s.IsCancelled);
                        break;

                    case SubscriptionStatus.Upcoming:
                        builder.Where(s => !s.IsCancelled && s.StartDate > now);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmed = search.Trim();
                builder.Where(s => s.PackageName.Contains(trimmed)
                              || s.Customer.DomainUser.Name.Contains(trimmed)
                              || (s.Customer.DomainUser.PhoneNumber != null && s.Customer.DomainUser.PhoneNumber.Contains(trimmed)));
            }
        }
    }
}
