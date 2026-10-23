using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.TrainerKpis.Commands.SubmitTrainerKpiEvaluations
{
    [Authorize(Permissions = [Permission.TrainersWrite])]
    public class SubmitTrainerKpiEvaluationsCommand : ICommand<SubmitTrainerKpiEvaluationsCommandResponse>, ITrainerResourceRequest
    {
        public int TrainerId { get; set; }
        public List<KpiScoreInputDto> Scores { get; set; } = [];
    }
}
