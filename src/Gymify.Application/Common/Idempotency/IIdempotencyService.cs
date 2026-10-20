namespace Gymify.Application.Common.Idempotency
{
    public interface IIdempotencyService
    {
        Task<(bool HasSavedResponse, TResponse Response)> GetSavedResponseAsync<TResponse>(Guid requestId, CancellationToken cancellationToken = default);

        Task SaveResponseAsync<TResponse>(Guid requestId, string name, TResponse response, CancellationToken cancellationToken = default);
    }
}
