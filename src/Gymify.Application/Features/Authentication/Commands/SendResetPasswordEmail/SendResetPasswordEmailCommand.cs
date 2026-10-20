
namespace Gymify.Application.Features.Authentication.Commands.SendResetPasswordEmail
{
    public class SendResetPasswordEmailCommand() : ICommand
    {
        public string Email { get; set; } = null!;
    }
}
