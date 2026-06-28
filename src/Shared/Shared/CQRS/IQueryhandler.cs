using MediatR;

namespace Shared.CQRS;

public interface IQueryhandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
}
