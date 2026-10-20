
namespace Gymify.Application.Features.Authentication.Commands.ResendConfirmEmail
{
    public class ResendConfirmationEmailCommand() : ICommand
    {
        public string Email { get; set; } = null!;
    }

}
