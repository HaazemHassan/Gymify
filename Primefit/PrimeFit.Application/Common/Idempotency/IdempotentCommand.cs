
namespace PrimeFit.Application.Common.Idempotency
{

    public abstract class IdempotentCommand(Guid requestId) : ICommand, IIdempotentRequest
    {
        public Guid RequestId { get; } = requestId;
    }

    public abstract class IdempotentCommand<TResponse>(Guid requestId) : ICommand<TResponse>, IIdempotentRequest
    {
        public Guid RequestId { get; } = requestId;
    }

}
