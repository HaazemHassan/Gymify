using Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations;

namespace Gymify.API.Requests.Trainers.SubmitTrainerKpiEvaluations
{
    public class SubmitTrainerKpiEvaluationsRequest
    {
        public List<KpiScoreInputDto> Scores { get; set; } = [];
    }
}
