using Gymify.Api.Requests.Common.Pagination;

namespace Gymify.API.Requests.Trainers.GetTrainerKpiEvaluations
{
    public class GetTrainerKpiEvaluationsRequest : BasicPaginationRequest
    {
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
    }
}
