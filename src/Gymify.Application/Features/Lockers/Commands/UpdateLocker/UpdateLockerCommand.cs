using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Lockers.Commands.UpdateLocker
{
    [Authorize(Permissions = [Permission.LockersWrite])]
    public class UpdateLockerCommand : ICommand<UpdateLockerCommandResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool? IsActive { get; set; }

        public UpdateLockerCommand()
        {
        }

        public UpdateLockerCommand(int id, int number, bool? isActive = null)
        {
            Id = id;
            Number = number;
            IsActive = isActive;
        }
    }
}
