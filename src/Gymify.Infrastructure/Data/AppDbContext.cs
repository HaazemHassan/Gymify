using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gymify.Domain.Entities;
using Gymify.Infrastructure.Data.Filters;
using Gymify.Infrastructure.Data.Identity.Entities;
using Gymify.Infrastructure.Data.Outbox;
using Gymify.Infrastructure.Idempotency;
using System.Reflection;

namespace Gymify.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<DomainUser> DomainUsers => Set<DomainUser>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
        public DbSet<IdempotentRequest> IdempotentRequests => Set<IdempotentRequest>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SoftDeleteQueryFilter.Apply(modelBuilder);
        }
    }
}