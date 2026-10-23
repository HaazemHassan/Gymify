namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerKpiEvaluations
{
    public class TrainerKpiAverageItemResponse
    {
        public int KpiId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal AverageScore { get; set; }
        public int EvaluationCount { get; set; }
    }
}
