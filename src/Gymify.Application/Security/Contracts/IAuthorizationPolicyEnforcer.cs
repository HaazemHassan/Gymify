
namespace Gymify.Application.Security.Contracts
{
    public interface IAuthorizationPolicyEnforcer
    {
        ErrorOr<Success> Authorize<TRequest>(TRequest request, string policyName);
    }
}
