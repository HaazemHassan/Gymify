
namespace PrimeFit.Application.Common.CQRS;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
