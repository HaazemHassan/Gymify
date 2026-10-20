using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public Permission[] Permissions { get; set; } = [];
        public UserRole[] Roles { get; set; } = [];
        public UserType[] UserTypes { get; set; } = [];

        public string? Policy { get; set; }

    }
}
