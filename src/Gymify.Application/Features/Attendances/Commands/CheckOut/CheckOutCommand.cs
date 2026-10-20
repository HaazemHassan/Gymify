using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Attendances.Commands.CheckOut
{
    [Authorize(Permissions = [Permission.AttendanceCheckOut])]
    public class CheckOutCommand : ICommand<CheckOutCommandResponse>, IAuthorizedRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
