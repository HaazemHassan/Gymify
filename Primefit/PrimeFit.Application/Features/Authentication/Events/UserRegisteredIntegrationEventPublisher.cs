using PrimeFit.Domain.DomainEvents;

namespace PrimeFit.Application.Features.Authentication.Events
{
    public sealed class UserRegisteredIntegrationEventPublisher : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly IOutboxIntegrationEventPublisher _publisher;

        public UserRegisteredIntegrationEventPublisher(IOutboxIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserRegisteredIntegrationEvent(notification.Email);

            _publisher.Publish(integrationEvent);

            return Task.CompletedTask;
        }
    }
}
