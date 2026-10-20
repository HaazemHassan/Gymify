using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Attendances.Commands.CheckIn
{
    [Authorize(Permissions = [Permission.AttendanceCheckIn])]
    public class CheckInCommand : ICommand<CheckInCommandResponse>, IAuthorizedRequest
    {
        public string Code { get; set; } = string.Empty;
        public int LockerId { get; set; }
    }
}
