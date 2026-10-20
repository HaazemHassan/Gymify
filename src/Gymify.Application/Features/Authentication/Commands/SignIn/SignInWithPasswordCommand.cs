using Gymify.Application.Features.Authentication.Common;

namespace Gymify.Application.Features.Authentication.Commands.SignIn;

public class SignInWithPasswordCommand : ICommand<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
