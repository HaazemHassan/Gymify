namespace Gymify.Application.Common.Messaging
{
    public interface IOutboxIntegrationEventPublisher
    {
        void Publish(IIntegrationEvent integrationEvent);
    }
}
