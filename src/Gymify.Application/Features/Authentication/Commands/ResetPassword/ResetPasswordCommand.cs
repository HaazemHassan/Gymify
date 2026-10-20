
namespace Gymify.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommand : ICommand
    {
        public string Email { get; init; }
        public string Code { get; init; }
        public string NewPassword { get; init; }
        public string ConfirmNewPassword { get; init; }
    }
}
