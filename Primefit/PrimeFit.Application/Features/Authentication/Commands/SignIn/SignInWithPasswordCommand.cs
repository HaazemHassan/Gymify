using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Authentication.Commands.SignIn;

public class SignInWithPasswordCommand : ICommand<AuthResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserType UserType { get; set; }
}
