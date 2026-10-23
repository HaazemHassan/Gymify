using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Gymify.Domain.Repositories;

namespace Gymify.Domain.RepositoriesContracts
{
    public class PaymentMethodRevenueAggregate
    {
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public int SubscriptionsCount { get; set; }

        public PaymentMethodRevenueAggregate()
        {
        }

        public PaymentMethodRevenueAggregate(PaymentMethod paymentMethod, decimal totalAmount, int subscriptionsCount)
        {
            PaymentMethod = paymentMethod;
            TotalAmount = totalAmount;
            SubscriptionsCount = subscriptionsCount;
        }
    }

    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<List<PaymentMethodRevenueAggregate>> GetPaymentMethodsRevenueSummaryAsync(
            DateTimeOffset? fromDate,
            DateTimeOffset? toDate,
            CancellationToken ct = default);
    }
}

