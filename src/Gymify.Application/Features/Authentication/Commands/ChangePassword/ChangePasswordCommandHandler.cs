using Gymify.Application.Contracts.Api;
using Gymify.Application.Contracts.Infrastructure;

namespace Gymify.Application.Features.Authentication.Commands.ChangePassword
{

    public class ChangePasswordCommandHandler(IPasswordService _passwordService, ICurrentUserService _currentUserService)
        : ICommandHandler<ChangePasswordCommand>
    {

        public async Task<ErrorOr<Success>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            return await _passwordService.ChangePassword(userId!.Value, request.CurrentPassword, request.NewPassword);
        }
    }
}
