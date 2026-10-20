using Gymify.Domain.Primitives;

namespace Gymify.Domain.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(string Email) : IDomainEvent;
}
