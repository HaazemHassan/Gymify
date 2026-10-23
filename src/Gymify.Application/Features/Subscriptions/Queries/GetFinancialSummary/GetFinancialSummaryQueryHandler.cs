using ErrorOr;
using Gymify.Application.Common.CQRS;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.RepositoriesContracts;

namespace Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary
{
    public class GetFinancialSummaryQueryHandler : IQueryHandler<GetFinancialSummaryQuery, GetFinancialSummaryQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFinancialSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetFinancialSummaryQueryResponse>> Handle(GetFinancialSummaryQuery request, CancellationToken cancellationToken)
        {
            var rawAggregates = await _unitOfWork.Subscriptions.GetPaymentMethodsRevenueSummaryAsync(
                request.FromDate,
                request.ToDate,
                cancellationToken);

            var totalRevenue = rawAggregates.Sum(a => a.TotalAmount);
            var totalCount = rawAggregates.Sum(a => a.SubscriptionsCount);

            var aggregatesDict = rawAggregates.ToDictionary(a => a.PaymentMethod);
            var paymentMethodsBreakdown = new List<PaymentMethodFinancialSummaryResponse>();

            foreach (var method in Enum.GetValues<PaymentMethod>())
            {
                aggregatesDict.TryGetValue(method, out var item);
                var methodAmount = item?.TotalAmount ?? 0m;
                var methodCount = item?.SubscriptionsCount ?? 0;
                var percentage = totalRevenue > 0
                    ? Math.Round((double)(methodAmount / totalRevenue * 100), 2)
                    : 0;

                paymentMethodsBreakdown.Add(new PaymentMethodFinancialSummaryResponse
                {
                    PaymentMethod = method,
                    PaymentMethodName = method.ToString(),
                    TotalAmount = methodAmount,
                    SubscriptionsCount = methodCount,
                    Percentage = percentage
                });
            }

            return new GetFinancialSummaryQueryResponse
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                TotalRevenue = totalRevenue,
                TotalSubscriptionsCount = totalCount,
                PaymentMethods = paymentMethodsBreakdown
            };
        }
    }
}

