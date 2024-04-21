namespace Shark.Domain.Base;
public interface ICommandHandler<TCommand>
{
    Task HandleAsync(TCommand command);
}

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}