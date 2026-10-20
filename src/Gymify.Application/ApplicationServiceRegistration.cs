using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Gymify.Application.Common;
using Gymify.Application.Common.Caching;
using Gymify.Application.Common.Cashing;
using Gymify.Application.Common.Idempotency;
using Gymify.Application.Common.Logging;
using Gymify.Application.Common.Transaction;
using Gymify.Application.Common.Trimming;
using Gymify.Application.Common.Validation;
using Gymify.Application.Security;
using Gymify.Application.Security.Policies;
using System.Reflection;

namespace Gymify.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IInMemoryEventDispatcher, InMemoryEventDispatcher>();

            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);

                cfg.AddOpenBehavior(typeof(TrimmingBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(IdempotentCommandPipelineBehavior<,>));
                cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
                cfg.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(PostCommitBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            AddAuthorizationPolicies(services);
            AddCachePolicies(services, typeof(ICachePolicy<>));
            AddCachePolicies(services, typeof(ICacheInvalidationPolicy<>));

            return services;
        }

        private static void AddAuthorizationPolicies(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationPolicy, SelfOrAdminPolicy>();
            services.AddScoped<IAuthorizationPolicy, SelfOnlyPolicy>();
        }

        private static void AddCachePolicies(IServiceCollection services, Type openGenericPolicyInterface)
        {
            var assembly = typeof(ApplicationAssemblyMarker).Assembly;

            var implementations = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .SelectMany(t => t.GetInterfaces(),
                    (implementation, iface) => new
                    {
                        Implementation = implementation,
                        Interface = iface
                    })
                .Where(x =>
                    x.Interface.IsGenericType &&
                    x.Interface.GetGenericTypeDefinition() == openGenericPolicyInterface);

            foreach (var type in implementations)
            {
                services.AddTransient(type.Interface, type.Implementation);
            }
        }
    }
}