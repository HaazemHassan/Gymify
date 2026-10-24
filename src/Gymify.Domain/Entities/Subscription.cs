using ErrorOr;
using Gymify.Domain.Common.Constants;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Subscription : FullAuditableEntity<int>
    {
        private Subscription()
        {
        }

        public Subscription(
            int customerId,
            int packageId,
            string packageName,
            decimal packagePrice,
            int packageDuration,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            PaymentMethod paymentMethod,
            SubscriptionType type = SubscriptionType.Regular,
            int? trainerId = null,
            string? packageDescription = null,
            string? notes = null)
        {
            CustomerId = customerId;
            PackageId = packageId;
            PackageName = packageName;
            PackagePrice = packagePrice;
            PackageDuration = packageDuration;
            StartDate = startDate;
            EndDate = endDate;
            PaymentMethod = paymentMethod;
            Type = type;
            TrainerId = type == SubscriptionType.Private ? trainerId : null;
            PackageDescription = packageDescription;
            Notes = notes;
        }

        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; } = null!;

        public int PackageId { get; private set; }
        public Package Package { get; private set; } = null!;

        // Snapshot of package details at time of subscription
        public string PackageName { get; private set; } = string.Empty;
        public decimal PackagePrice { get; private set; }
        public int PackageDuration { get; private set; }
        public string? PackageDescription { get; private set; }

        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; }
        public SubscriptionType Type { get; private set; } = SubscriptionType.Regular;
        public int? TrainerId { get; private set; }
        public Trainer? Trainer { get; private set; }
        public string? Notes { get; private set; }

        public bool IsCancelled { get; private set; }
        public DateTimeOffset? CancelledAt { get; private set; }
        public string? CancellationReason { get; private set; }

        public ICollection<SubscriptionFreeze> Freezes { get; private set; } = new List<SubscriptionFreeze>();

        public bool IsFrozen(DateTimeOffset now) => !IsCancelled && Freezes.Any(f => f.StartDate <= now && f.EndDate >= now);

        public bool IsActive(DateTimeOffset now) => !IsCancelled && StartDate <= now && EndDate >= now && !IsFrozen(now);

        public SubscriptionStatus GetStatus(DateTimeOffset now)
        {
            if (IsCancelled) return SubscriptionStatus.Cancelled;
            if (now < StartDate) return SubscriptionStatus.Upcoming;
            if (now > EndDate) return SubscriptionStatus.Expired;
            if (IsFrozen(now)) return SubscriptionStatus.Frozen;
            return SubscriptionStatus.Active;
        }

        public ErrorOr<SubscriptionFreeze> Freeze(DateTimeOffset now, DateTimeOffset endDate, string? reason = null)
        {
            if (IsCancelled)
            {
                return Error.Conflict(
                    code: ErrorCodes.Subscription.CannotFreezeCancelled,
                    description: "Cannot freeze a cancelled subscription.");
            }

            if (now > EndDate)
            {
                return Error.Conflict(
                    code: ErrorCodes.Subscription.CannotFreezeExpired,
                    description: "Cannot freeze an expired subscription.");
            }

            if (endDate <= now)
            {
                return Error.Validation(
                    code: ErrorCodes.Subscription.InvalidFreezeDates,
                    description: "Freeze end date must be after current date and time.");
            }

            var hasOverlap = Freezes.Any(f => now <= f.EndDate && endDate >= f.StartDate);
            if (hasOverlap)
            {
                return Error.Conflict(
                    code: ErrorCodes.Subscription.FreezeOverlap,
                    description: "The specified freeze period overlaps with an existing freeze on this subscription.");
            }

            var freezeDays = (int)Math.Max(1, Math.Ceiling((endDate - now).TotalDays));
            EndDate = EndDate.AddDays(freezeDays);

            var freeze = new SubscriptionFreeze(Id, now, endDate, reason);
            Freezes.Add(freeze);

            return freeze;
        }

        public ErrorOr<Success> Unfreeze(DateTimeOffset now)
        {
            if (IsCancelled)
            {
                return Error.Conflict(
                    code: ErrorCodes.Subscription.AlreadyCancelled,
                    description: "Cannot unfreeze a cancelled subscription.");
            }

            var activeFreeze = Freezes.FirstOrDefault(f => f.StartDate <= now && f.EndDate >= now);
            if (activeFreeze is null)
            {
                return Error.Conflict(
                    code: ErrorCodes.Subscription.NotFrozen,
                    description: "Subscription is not currently frozen.");
            }

            var plannedDays = (int)Math.Max(1, Math.Ceiling((activeFreeze.EndDate - activeFreeze.StartDate).TotalDays));
            var elapsedDays = (now - activeFreeze.StartDate).TotalDays;
            var actualDays = elapsedDays < 0.5 ? 0 : (int)Math.Max(1, Math.Ceiling(elapsedDays));
            actualDays = Math.Min(actualDays, plannedDays);

            var unusedDays = plannedDays - actualDays;
            if (unusedDays > 0)
            {
                EndDate = EndDate.AddDays(-unusedDays);
            }

            activeFreeze.EndEarly(now);

            return Result.Success;
        }

        public ErrorOr<Success> Cancel(DateTimeOffset cancelledAt, string? reason = null)
        {
            if (IsCancelled)
            {
                return Error.Conflict(
                    code: ErrorCodes.Subscription.AlreadyCancelled,
                    description: "Subscription is already cancelled.");
            }

            IsCancelled = true;
            CancelledAt = cancelledAt;
            CancellationReason = reason;

            return Result.Success;
        }
    }
}
