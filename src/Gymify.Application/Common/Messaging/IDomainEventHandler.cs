using Gymify.Domain.Primitives;

namespace Gymify.Application.Common.Messaging
{
    public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : IDomainEvent
    {
    }
}
