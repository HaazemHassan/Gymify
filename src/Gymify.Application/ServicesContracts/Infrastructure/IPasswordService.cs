using Gymify.Domain.Entities;

namespace Gymify.Application.Contracts.Infrastructure
{
    public interface IPasswordService
    {
        Task<ErrorOr<Success>> ChangePassword(int domainUserId, string currentPassword, string newPassword);
        Task<ErrorOr<string>> CreatePasswordResetCode(int domainUserId, CancellationToken ct = default);
        Task SendPasswordResetEmailAsync(DomainUser user, string code);
        Task<ErrorOr<bool>> IsPasswordResetCodeValid(string email, string code, CancellationToken ct = default);
        Task<ErrorOr<Success>> ResetPassword(string email, string code, string newPassword, CancellationToken ct = default);
    }
}
