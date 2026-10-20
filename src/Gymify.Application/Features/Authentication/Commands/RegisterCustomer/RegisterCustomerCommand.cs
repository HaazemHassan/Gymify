using Gymify.Application.Common.Transaction;

namespace Gymify.Application.Features.Authentication.Commands.RegisterCustomer
{
    public class RegisterCustomerCommand : ICommand<RegisterCustomerResponse>, ITransactionalRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
