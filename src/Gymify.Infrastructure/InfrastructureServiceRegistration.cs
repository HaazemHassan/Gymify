using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gymify.Application.Common.Idempotency;
using Gymify.Application.Common.Messaging;
using Gymify.Application.Common.Options;
using Gymify.Application.Contracts.Infrastructure;
using Gymify.Application.Security.Contracts;
using Gymify.Application.ServicesContracts.Infrastructure;
using Gymify.Application.ServicesContracts.Infrastructure.Caching;
using Gymify.Domain.Repositories;
using Gymify.Domain.RepositoriesContracts;
using Gymify.Infrastructure.BackgroundJobs;
using Gymify.Infrastructure.Caching;
using Gymify.Infrastructure.Common.Options;
using Gymify.Infrastructure.Data;
using Gymify.Infrastructure.Data.Identity.Entities;
using Gymify.Infrastructure.Data.Interceptors;
using Gymify.Infrastructure.Data.Repositories;
using Gymify.Infrastructure.Data.Seeding;
using Gymify.Infrastructure.Emails;
using Gymify.Infrastructure.Health;
using Gymify.Infrastructure.Idempotency;
using Gymify.Infrastructure.Messaging;
using Gymify.Infrastructure.Messaging.Jobs;

using Gymify.Infrastructure.Security;
using Gymify.Infrastructure.Security.Jobs;
using Gymify.Infrastructure.Services;
using Gymify.Infrastructure.Storage;
using Gymify.Infrastructure.Storage.Jobs;

namespace Gymify.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContextConfiguations(services, configuration);
        AddIdentityConfigurations(services, configuration);
        AddCashing(services, configuration);
        AddRepositories(services);
        AddServices(services, configuration);
        AddHangfireConfiguration(services, configuration);
        AddBackgroundJobs(services, configuration);
        AddHealthChecks(services, configuration);

        return services;
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddHealthChecks()
                .AddSqlServer(dbConnectionString)
                .AddCheck<CloudinaryHealthCheck>(
                    name: "cloudinary",
                    tags: ["storage", "external"]
                 );
    }

    private static void AddDbContextConfiguations(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<PublishDomainEventsInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(
                configuration["ConnectionStrings:DefaultConnection"],
                x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("__EFMigrationsHistory", "ef");
                });

            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditingInterceptor>(),
                serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>()
            );
        });

        services.AddScoped<AuditingInterceptor>();
    }

    private static IServiceCollection AddIdentityConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppPasswordOptions>(configuration.GetSection(AppPasswordOptions.SectionName));

        var passwordOptions = configuration.GetSection(AppPasswordOptions.SectionName).Get<AppPasswordOptions>();

        services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
        {
            option.Password.RequireDigit = passwordOptions?.RequireDigit ?? true;
            option.Password.RequireLowercase = passwordOptions?.RequireLowercase ?? true;
            option.Password.RequireNonAlphanumeric = passwordOptions?.RequireNonAlphanumeric ?? true;
            option.Password.RequireUppercase = passwordOptions?.RequireUppercase ?? true;
            option.Password.RequiredLength = passwordOptions?.MinLength ?? 8;
            option.Password.RequiredUniqueChars = passwordOptions?.RequiredUniqueChars ?? 1;

            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            option.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddPasswordValidator<CustomPasswordValidator>();

        return services;
    }

    private static IServiceCollection AddHangfireConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HangfireOptions>(configuration.GetSection(HangfireOptions.SectionName));

        services.AddHangfire(config => config
         .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
         .UseSimpleAssemblyNameTypeSerializer()
         .UseRecommendedSerializerSettings()
         .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new Hangfire.SqlServer.SqlServerStorageOptions
         {
             SchemaName = "jobs"
         }));

        services.AddHangfireServer();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.AddScoped<RefreshTokensCleanupJob>();
        services.AddScoped<IImageBackgroundService, HangfireImageBackgroundService>();
        services.AddScoped<ImageCleanupJob>();
        services.AddScoped<ProcessOutboxMessagesJob>();

        return services;
    }

    private static IServiceCollection AddCashing(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1 * 1024 * 1024;

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(1)
            };
        });

        services.AddScoped<ICacheService, HybridCacheService>();

        return services;
    }

    private static IServiceCollection AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();

        return services;
    }

    private static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISeederService, SeederService>();
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IEmailVerificationService, EmailVerificationService>();
        services.AddScoped<IAuthorizationPolicyEnforcer, PolicyEnforcer>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();

        services.AddSingleton<IPhoneNumberService, PhoneNumberService>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IOtpService, OtpService>();
        services.AddScoped<IOutboxIntegrationEventPublisher, OutboxIntegrationEventPublisher>();

        services.Configure<GoogleAuthOptions>(configuration.GetSection(GoogleAuthOptions.SectionName));
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();

        services.Configure<CloudinaryOptions>(configuration.GetSection(CloudinaryOptions.SectionName));
        services.AddScoped<IImageService, CloudinaryImageService>();

        services.Configure<EmailVerificationCodeOptions>(configuration.GetSection(EmailVerificationCodeOptions.SectionName));
        services.Configure<MailOptions>(configuration.GetSection(MailOptions.SectionName));

        services.AddSingleton<IEmailBodyBuilderService, EmailBodyBuilderService>();
        services.AddScoped<IEmailService, EmailService>();


        return services;
    }




}
