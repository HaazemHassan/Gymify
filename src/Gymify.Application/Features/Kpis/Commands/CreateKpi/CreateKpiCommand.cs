using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Kpis.Commands.CreateKpi
{
    [Authorize(Permissions = [Permission.TrainersWrite])]
    public class CreateKpiCommand : ICommand<CreateKpiCommandResponse>, IAuthorizedRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
