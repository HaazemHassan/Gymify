namespace Gymify.Application.Common.Idempotency
{
    public interface IIdempotentRequest
    {
        Guid RequestId { get; }
    }
}
