using MediatR;

namespace Gymify.Application.Common.Idempotency;

internal sealed class IdempotentCommandPipelineBehavior<TRequest, TResponse>(
    IIdempotencyService idempotencyService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentRequest
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var (hasSavedResponse, savedResponse) = await idempotencyService.GetSavedResponseAsync<TResponse>(
            request.RequestId, cancellationToken);

        if (hasSavedResponse)
        {
            return savedResponse;
        }

        var response = await next(cancellationToken);

        if (response is IErrorOr { IsError: false })
        {
            await idempotencyService.SaveResponseAsync(
                request.RequestId,
                typeof(TRequest).Name,
                response,
                cancellationToken);
        }

        return response;
    }
}