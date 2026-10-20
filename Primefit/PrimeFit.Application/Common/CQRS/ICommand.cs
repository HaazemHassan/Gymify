
namespace PrimeFit.Application.Common.CQRS;

public interface IBaseCommand { }

public interface ICommand : IRequest<ErrorOr<Success>>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>, IBaseCommand
{
}
