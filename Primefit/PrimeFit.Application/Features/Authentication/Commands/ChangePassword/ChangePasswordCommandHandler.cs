using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;

namespace PrimeFit.Application.Features.Authentication.Commands.ChangePassword
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
