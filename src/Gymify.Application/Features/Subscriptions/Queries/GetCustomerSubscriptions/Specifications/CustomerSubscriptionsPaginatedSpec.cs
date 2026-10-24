using Ardalis.Specification;
using Gymify.Application.Features.Subscriptions.Specifications;
using Gymify.Application.Specifications;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Subscriptions.Queries.GetCustomerSubscriptions.Specifications
{
    public class CustomerSubscriptionsPaginatedSpec : Specification<Subscription, GetCustomerSubscriptionsQueryResponse>
    {
        public CustomerSubscriptionsPaginatedSpec(
            int customerId,
            int pageNumber,
            int pageSize,
            DateTimeOffset now,
            SubscriptionStatus? status = null,
            string? search = null,
            SubscriptionType? type = null)
        {
            SubscriptionsFilteredSpec.ApplyFilters(Query, now, status, search, type, customerId);

            Query.OrderByDescending(s => s.CreatedAt);
            Query.Paginate(pageNumber, pageSize);

            Query.Select(s => new GetCustomerSubscriptionsQueryResponse
            {
                Id = s.Id,
                CustomerId = s.CustomerId,
                CustomerName = s.Customer.DomainUser.Name,
                CustomerPhone = s.Customer.DomainUser.PhoneNumber,
                PackageId = s.PackageId,
                PackageName = s.PackageName,
                PackagePrice = s.PackagePrice,
                PackageDuration = s.PackageDuration,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                PaymentMethod = s.PaymentMethod,
                Type = s.Type,
                TrainerId = s.TrainerId,
                TrainerName = s.Trainer != null ? s.Trainer.Employee.DomainUser.Name : null,
                Status = s.IsCancelled ? SubscriptionStatus.Cancelled :
                         now < s.StartDate ? SubscriptionStatus.Upcoming :
                         now > s.EndDate ? SubscriptionStatus.Expired :
                         s.Freezes.Any(f => f.StartDate <= now && f.EndDate >= now) ? SubscriptionStatus.Frozen :
                         SubscriptionStatus.Active,
                IsCancelled = s.IsCancelled,
                CancelledAt = s.CancelledAt,
                CancellationReason = s.CancellationReason,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt
            });
        }
    }
}
