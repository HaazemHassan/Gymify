using Gymify.Application.Features.Users.Common;
using System.Text.Json.Serialization;

namespace Gymify.Application.Features.Authentication.Common
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshTokenDTO? RefreshToken { get; set; }

        public UserBaseResponse User { get; set; } = null!;

        public AuthResponse()
        {
        }

        public AuthResponse(AuthResult authResult, UserBaseResponse user)
        {
            AccessToken = authResult.AccessToken;
            RefreshToken = authResult.RefreshToken;
            User = user;
        }
    }
}
