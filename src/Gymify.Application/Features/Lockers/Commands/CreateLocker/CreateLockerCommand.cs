using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Lockers.Commands.CreateLocker
{
    [Authorize(Permissions = [Permission.LockersWrite])]
    public class CreateLockerCommand : ICommand<CreateLockerCommandResponse>, IAuthorizedRequest
    {
        public int Number { get; set; }

        public CreateLockerCommand()
        {
        }

        public CreateLockerCommand(int number)
        {
            Number = number;
        }
    }
}
