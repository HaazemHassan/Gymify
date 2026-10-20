using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Gymify.Application.Common.Idempotency;
using Gymify.Infrastructure.Data;
using System.Text.Json;

namespace Gymify.Infrastructure.Idempotency;

internal sealed class IdempotencyService(AppDbContext context) : IIdempotencyService
{
    public async Task<(bool HasSavedResponse, TResponse Response)> GetSavedResponseAsync<TResponse>(Guid requestId, CancellationToken cancellationToken = default)
    {
        var request = await context.Set<IdempotentRequest>()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request is null)
        {
            return (false, default!);
        }

        if (request.Response is not null && request.ResponseType is not null)
        {
            var valueType = Type.GetType(request.ResponseType);
            if (valueType is not null)
            {
                var deserializedValue = JsonSerializer.Deserialize(request.Response, valueType);
                if (deserializedValue is not null)
                {
                    return (true, (TResponse)(dynamic)deserializedValue);
                }
            }
        }

        return (true, (TResponse)(dynamic)Result.Success);
    }

    public async Task SaveResponseAsync<TResponse>(Guid requestId, string name, TResponse response, CancellationToken cancellationToken = default)
    {

        string? responseJson = null;
        string? responseType = null;

        if (response is IErrorOr errorOr)
        {
            var value = errorOr.GetType().GetProperty("Value")?.GetValue(errorOr);

            if (value is not null && value.GetType().Name != "Success")
            {
                var actualType = value.GetType();
                responseJson = JsonSerializer.Serialize(value, actualType);
                responseType = $"{actualType.FullName}, {actualType.Assembly.GetName().Name}";
            }
        }

        context.Add(new IdempotentRequest
        {
            Id = requestId,
            Name = name,
            Response = responseJson,
            ResponseType = responseType
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}
