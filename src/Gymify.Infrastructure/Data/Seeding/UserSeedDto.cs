using Gymify.Domain.Common.Enums;

namespace Gymify.Infrastructure.Data.Seeding
{
    public class UserSeedDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole? Role { get; set; }
        public UserType UserType { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
