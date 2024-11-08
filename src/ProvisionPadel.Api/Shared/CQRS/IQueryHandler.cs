namespace ProvisionPadel.Api.Shared.CQRS;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse> where TResponse : notnull
{
}