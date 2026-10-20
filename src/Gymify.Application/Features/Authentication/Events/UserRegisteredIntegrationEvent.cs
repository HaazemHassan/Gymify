
namespace Gymify.Application.Features.Authentication.Events
{
    public sealed record UserRegisteredIntegrationEvent(string Email) : IIntegrationEvent;
}
