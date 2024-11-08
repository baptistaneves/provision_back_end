﻿using Mapster;

namespace ProvisionPadel.Api.Features.Roles.Remove;

public record RemoveRoleResponse(bool IsSuccess);

public class RemoveRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/role/remove/{id}", async (Guid id, ISender sender) =>
        {
            var command = new RemoveRoleCommand(id);

            var result = await sender.Send(command);

            var response = result.Adapt<RemoveRoleResponse>();

            return Results.Ok(response);
        })
        .WithName("RemoveRole")
        .Produces<RemoveRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithDescription("Remove Role");
    }
}