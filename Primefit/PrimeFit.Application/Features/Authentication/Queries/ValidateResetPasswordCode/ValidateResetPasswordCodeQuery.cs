
namespace PrimeFit.Application.Features.Authentication.Queries.ValidateResetPasswordCode
{
    public class ValidateResetPasswordCodeQuery : IQuery<ValidateResetPasswordCodeQueryResponse>
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
