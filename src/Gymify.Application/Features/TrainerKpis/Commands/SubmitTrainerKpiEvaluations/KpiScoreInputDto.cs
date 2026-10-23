namespace Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations
{
    public class KpiScoreInputDto
    {
        public int KpiId { get; set; }
        public decimal Score { get; set; }
        public string? Comment { get; set; }
    }
}
