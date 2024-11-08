namespace ProvisionPadel.Api.Shared.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}
