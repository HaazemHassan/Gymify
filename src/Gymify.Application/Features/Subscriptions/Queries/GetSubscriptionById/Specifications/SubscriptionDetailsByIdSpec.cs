using Ardalis.Specification;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Subscriptions.Queries.GetSubscriptionById.Specifications
{
    public class SubscriptionDetailsByIdSpec : Specification<Subscription, GetSubscriptionByIdQueryResponse>, ISingleResultSpecification<Subscription, GetSubscriptionByIdQueryResponse>
    {
        public SubscriptionDetailsByIdSpec(int subscriptionId, DateTimeOffset now)
        {
            Query.Where(s => s.Id == subscriptionId);

            Query.Select(s => new GetSubscriptionByIdQueryResponse
            {
                Id = s.Id,
                CustomerId = s.CustomerId,
                CustomerName = s.Customer.DomainUser.Name,
                CustomerPhone = s.Customer.DomainUser.PhoneNumber,
                PackageId = s.PackageId,
                PackageName = s.PackageName,
                PackagePrice = s.PackagePrice,
                PackageDuration = s.PackageDuration,
                PackageDescription = s.PackageDescription,
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
                CreatedAt = s.CreatedAt,
                Freezes = s.Freezes
                    .OrderByDescending(f => f.StartDate)
                    .Select(f => new SubscriptionFreezeDetailsDto
                    {
                        Id = f.Id,
                        StartDate = f.StartDate,
                        EndDate = f.EndDate,
                        Reason = f.Reason,
                        IsActive = f.StartDate <= now && f.EndDate >= now,
                        CreatedAt = f.CreatedAt
                    })
                    .ToList()
            });
        }
    }
}
