namespace Gymify.API.Requests.Subscriptions.GetFinancialSummary
{
    public class GetFinancialSummaryRequest
    {
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
    }
}
