
namespace Gymify.Application.Common.CQRS;

public interface IBaseQuery { }

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>, IBaseQuery
{
}
