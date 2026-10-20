
namespace Gymify.Application.Security.Policies
{
    public interface IAuthorizationPolicy
    {
        string Name { get; }
        ErrorOr<Success> Authorize(object request);
    }
}
