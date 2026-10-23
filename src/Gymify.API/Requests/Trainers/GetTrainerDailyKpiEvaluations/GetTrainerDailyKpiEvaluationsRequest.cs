using Gymify.Api.Requests.Common.Pagination;

namespace Gymify.API.Requests.Trainers.GetTrainerDailyKpiEvaluations
{
    public class GetTrainerDailyKpiEvaluationsRequest : BasicPaginationRequest
    {
        public DateOnly? Date { get; set; }
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
    }
}
