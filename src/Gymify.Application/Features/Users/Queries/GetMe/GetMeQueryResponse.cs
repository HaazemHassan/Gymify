using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Users.Queries.GetMe
{
    public class GetMeQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public UserRole? UserRole { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
