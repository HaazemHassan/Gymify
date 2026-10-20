using Gymify.Application.Common.Messaging;
using Gymify.Infrastructure.Data;
using Gymify.Infrastructure.Data.Outbox;
using System.Text.Json;

namespace Gymify.Infrastructure.Messaging
{
    public class OutboxIntegrationEventPublisher : IOutboxIntegrationEventPublisher
    {
        private readonly AppDbContext _dbContext;

        public OutboxIntegrationEventPublisher(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Publish(IIntegrationEvent integrationEvent)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = integrationEvent.GetType().Name,
                Content = JsonSerializer.Serialize(
                    integrationEvent,
                    integrationEvent.GetType(),
                    new JsonSerializerOptions
                    {
                        WriteIndented = false
                    })
            };

            _dbContext.Set<OutboxMessage>().Add(outboxMessage);
        }
    }
}
