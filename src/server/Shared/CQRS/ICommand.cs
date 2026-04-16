namespace Genovel.Shared.CQRS;

using MediatR;

public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out TResponse> : IRequest<TResponse>
{

}
