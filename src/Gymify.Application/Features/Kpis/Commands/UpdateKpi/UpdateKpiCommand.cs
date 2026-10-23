using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Kpis.Commands.UpdateKpi
{
    [Authorize(Permissions = [Permission.TrainersWrite])]
    public class UpdateKpiCommand : ICommand<UpdateKpiCommandResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
