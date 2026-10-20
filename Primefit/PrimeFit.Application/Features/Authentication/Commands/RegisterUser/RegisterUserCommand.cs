using PrimeFit.Application.Common.Transaction;
using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommand : ICommand<UserBaseResponse>, ITransactionalRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

    }
}
