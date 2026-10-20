namespace Gymify.Domain.Entities.Contracts
{
    public interface IHasCreationTime
    {
        DateTimeOffset CreatedAt { get; }
    }
}
