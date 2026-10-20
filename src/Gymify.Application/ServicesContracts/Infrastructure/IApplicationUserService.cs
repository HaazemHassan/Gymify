using Gymify.Application.Features.Users.Common;
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;

namespace Gymify.Application.ServicesContracts.Infrastructure
{
    public interface IApplicationUserService
    {
        Task<ErrorOr<int>> AddUser(DomainUser user, string password, UserRole? userRole = default, CancellationToken ct = default);
        Task<ErrorOr<UserBaseResponse>> GetUserAuthProfileAsync(int domainUserId, CancellationToken ct = default);
        Task<bool> IsEmailConfirmedAsync(int domainUserId, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateUnconfirmedUserAsync(DomainUser domainUser, string newPassword, CancellationToken ct = default);
    }
}