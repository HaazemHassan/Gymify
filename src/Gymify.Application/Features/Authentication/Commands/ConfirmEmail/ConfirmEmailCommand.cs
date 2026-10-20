
namespace Gymify.Application.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : ICommand
    {
        public string Email { get; init; }
        public string Code { get; init; }
    }

}
