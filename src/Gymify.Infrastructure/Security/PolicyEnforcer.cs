using ErrorOr;
using Gymify.Application.Security.Contracts;
using Gymify.Application.Security.Policies;
using Gymify.Domain.Common.Constants;

namespace Gymify.Infrastructure.Security
{
    internal class PolicyEnforcer : IAuthorizationPolicyEnforcer
    {
        private readonly IEnumerable<IAuthorizationPolicy> _policies;

        public PolicyEnforcer(IEnumerable<IAuthorizationPolicy> policies)
        {
            _policies = policies;
        }
        public ErrorOr<Success> Authorize<TRequest>(TRequest request, string policyName)
        {
            var policy = _policies.FirstOrDefault(p => p.Name == policyName);

            if (policy is null)
            {
                return Error.Failure(
                    code: ErrorCodes.Authorization.UnknownPolicy,
                    description: $"Unknown policy: {policyName}");
            }

            return policy.Authorize(request!);
        }


    }
}
