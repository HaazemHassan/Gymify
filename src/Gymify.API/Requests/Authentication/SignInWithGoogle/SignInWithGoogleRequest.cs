using Gymify.Domain.Common.Enums;
using ErrorOr;

namespace Gymify.Api.Requests.Authentication.SignInWithGoogle;

public class SignInWithGoogleRequest
{
        public string IdToken { get; set; }
        public UserType UserType { get; set; }
}

