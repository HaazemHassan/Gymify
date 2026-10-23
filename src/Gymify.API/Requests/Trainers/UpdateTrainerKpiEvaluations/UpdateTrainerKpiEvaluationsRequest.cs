using Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations;

namespace Gymify.API.Requests.Trainers.UpdateTrainerKpiEvaluations
{
    public class UpdateTrainerKpiEvaluationsRequest
    {
        public DateOnly Date { get; set; }
        public List<KpiScoreInputDto> Scores { get; set; } = [];
    }
}
