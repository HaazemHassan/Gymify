using Microsoft.EntityFrameworkCore.Storage;
using Gymify.Domain.Repositories;
using Gymify.Domain.RepositoriesContracts;
using Gymify.Infrastructure.Data.Transactions;

namespace Gymify.Infrastructure.Data;

internal sealed class UnitOfWork(
    AppDbContext context,
    IUserRepository users,
    ICustomerRepository customers,
    IEmployeeRepository employees,
    IPackageRepository packages,
    ITrainerRepository trainers,
    ILockerRepository lockers,
    IAttendanceRepository attendances,
    ISubscriptionRepository subscriptions,
    ICustomerBehaviorNoteRepository customerBehaviorNotes,
    IRefreshTokenRepository refreshTokens,
    IVerificationCodeRepository verificationCodes,
    IKpiRepository kpis,
    ITrainerKpiScoreRepository trainerKpiScores)
    : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public IUserRepository Users { get; } = users;
    public ICustomerRepository Customers { get; } = customers;
    public IEmployeeRepository Employees { get; } = employees;
    public IPackageRepository Packages { get; } = packages;
    public ITrainerRepository Trainers { get; } = trainers;
    public ILockerRepository Lockers { get; } = lockers;
    public IAttendanceRepository Attendances { get; } = attendances;
    public ISubscriptionRepository Subscriptions { get; } = subscriptions;
    public ICustomerBehaviorNoteRepository CustomerBehaviorNotes { get; } = customerBehaviorNotes;
    public IRefreshTokenRepository RefreshTokens { get; } = refreshTokens;
    public IVerificationCodeRepository VerificationCodes { get; } = verificationCodes;
    public IKpiRepository Kpis { get; } = kpis;
    public ITrainerKpiScoreRepository TrainerKpiScores { get; } = trainerKpiScores;

    private IDbContextTransaction? _transaction;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        return new DatabaseTransaction(_transaction);
    }

    public bool HasCurrentTransaction()
    {
        return _context.Database.CurrentTransaction != null;
    }

    public IDatabaseExecutionStrategy CreateExecutionStrategy()
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return new DatabaseExecutionStrategy(strategy);
    }
}