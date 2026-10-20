using ErrorOr;

namespace Gymify.Api.Requests.Authentication.ValidateResetPasswordCode;

public class ValidateResetPasswordCodeRequest
{
        public string Email { get; set; }
        public string Code { get; set; }
}
