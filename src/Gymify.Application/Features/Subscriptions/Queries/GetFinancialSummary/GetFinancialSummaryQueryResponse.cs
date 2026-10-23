using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary
{
    public class GetFinancialSummaryQueryResponse
    {
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalSubscriptionsCount { get; set; }
        public List<PaymentMethodFinancialSummaryResponse> PaymentMethods { get; set; } = [];
    }

    public class PaymentMethodFinancialSummaryResponse
    {
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int SubscriptionsCount { get; set; }
        public double Percentage { get; set; }
    }
}
