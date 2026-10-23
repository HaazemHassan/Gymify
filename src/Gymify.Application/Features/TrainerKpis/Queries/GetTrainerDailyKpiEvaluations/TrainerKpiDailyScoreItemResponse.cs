namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerDailyKpiEvaluations
{
    public class TrainerKpiDailyScoreItemResponse
    {
        public int KpiId { get; set; }
        public string KpiName { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public string? Comment { get; set; }
    }
}
