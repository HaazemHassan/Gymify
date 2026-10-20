using Gymify.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace Gymify.Application.Features.Users.Common
{
    public class UserBaseResponse
    {
        [JsonPropertyOrder(-1)]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public UserType UserType { get; set; }
        public UserRole? UserRole { get; set; }
        public bool IsActive { get; set; }
    }
}
