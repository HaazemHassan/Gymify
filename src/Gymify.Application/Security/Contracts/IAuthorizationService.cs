using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Security.Contracts
{
    public interface IAuthorizationService
    {
        ErrorOr<Success> AuthorizeCurrentUser<TRequest>(
            TRequest request,
            List<UserRole> requiredRoles,
            List<Permission> requiredPermissions,
            List<string> requiredPolicies,
            List<UserType> userTypes);
    }
}
