using Gymify.Application.Common.CQRS;
using Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations;
using Gymify.Application.Security;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.TrainerKpis.Commands.UpdateTrainerKpiEvaluations
{
    [Authorize(Permissions = [Permission.TrainersWrite])]
    public class UpdateTrainerKpiEvaluationsCommand : ICommand<UpdateTrainerKpiEvaluationsCommandResponse>
    {
        public int TrainerId { get; set; }
        public DateOnly Date { get; set; }
        public List<KpiScoreInputDto> Scores { get; set; } = [];
    }
}
