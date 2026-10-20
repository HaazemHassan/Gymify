using Gymify.Domain.Entities.Contracts;

namespace Gymify.Infrastructure.Idempotency
{
    public sealed class IdempotentRequest : IHasCreationTime
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Response { get; set; }

        public string? ResponseType { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
