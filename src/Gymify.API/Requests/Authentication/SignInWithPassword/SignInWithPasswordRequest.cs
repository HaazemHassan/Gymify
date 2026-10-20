using Gymify.Domain.Common.Enums;

namespace Gymify.Api.Requests.Authentication.SignInWithPassword;

public class SignInWithPasswordRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserType UserType { get; set; }
}

