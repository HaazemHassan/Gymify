using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Subscriptions.Queries.GetFinancialSummary
{
    [Authorize(Permissions = [Permission.SubscriptionsRead])]
    public class GetFinancialSummaryQuery : IQuery<GetFinancialSummaryQueryResponse>, IAuthorizedRequest
    {
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
    }
}
