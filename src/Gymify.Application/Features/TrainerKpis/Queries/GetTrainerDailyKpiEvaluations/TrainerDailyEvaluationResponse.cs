namespace Gymify.Application.Features.TrainerKpis.Queries.GetTrainerDailyKpiEvaluations
{
    public class TrainerDailyEvaluationResponse
    {
        public DateOnly Date { get; set; }
        public decimal DailyAverage { get; set; }
        public List<TrainerKpiDailyScoreItemResponse> Scores { get; set; } = [];
    }
}
