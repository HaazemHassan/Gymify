using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Lockers.Commands.ChangeLockerStatus
{
    [Authorize(Permissions = [Permission.LockersWrite])]
    public class ChangeLockerStatusCommand : ICommand<ChangeLockerStatusCommandResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

        public ChangeLockerStatusCommand()
        {
        }

        public ChangeLockerStatusCommand(int id, bool isActive)
        {
            Id = id;
            IsActive = isActive;
        }
    }
}
