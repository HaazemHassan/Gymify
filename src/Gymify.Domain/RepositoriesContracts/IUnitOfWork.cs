using Gymify.Domain.Repositories;

namespace Gymify.Domain.RepositoriesContracts;

public interface IUnitOfWork
{

    IUserRepository Users { get; }
    ICustomerRepository Customers { get; }
    IEmployeeRepository Employees { get; }
    IPackageRepository Packages { get; }
    ITrainerRepository Trainers { get; }
    ILockerRepository Lockers { get; }
    IAttendanceRepository Attendances { get; }
    ISubscriptionRepository Subscriptions { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IVerificationCodeRepository VerificationCodes { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    bool HasCurrentTransaction();

    IDatabaseExecutionStrategy CreateExecutionStrategy();

}
