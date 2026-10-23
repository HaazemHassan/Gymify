namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerKpiEvaluations
{
    public class GetTrainerKpiEvaluationsQueryResponse
    {
        public int TrainerId { get; set; }
        public EvaluationPeriodResponse Period { get; set; } = new();
        public decimal OverallAverage { get; set; }
        public List<TrainerKpiAverageItemResponse> Kpis { get; set; } = [];

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
