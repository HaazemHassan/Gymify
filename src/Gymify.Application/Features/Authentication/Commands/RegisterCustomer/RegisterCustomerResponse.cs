using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Authentication.Commands.RegisterCustomer
{
    public class RegisterCustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
