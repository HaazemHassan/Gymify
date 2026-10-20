using ErrorOr;

namespace Gymify.Application.Security.Contracts
{
    public interface IPolicyEnforcer
    {
        ErrorOr<Success> Authorize<TRequest>(TRequest request, string policyName);
    }
}
