﻿namespace ProvisionPadel.Api.Shared.CQRS;

public interface ICommand : IRequest<Unit>
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
