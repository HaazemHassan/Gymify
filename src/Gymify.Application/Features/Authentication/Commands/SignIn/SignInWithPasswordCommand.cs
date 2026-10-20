using Gymify.Application.Features.Authentication.Common;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Authentication.Commands.SignIn;

public class SignInWithPasswordCommand : ICommand<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserType UserType { get; set; }
}
