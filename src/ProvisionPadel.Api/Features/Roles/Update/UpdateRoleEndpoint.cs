using Mapster;

namespace ProvisionPadel.Api.Features.Roles.Update;

public record UpdateRoleResponse(bool IsSuccess);

public record UpdateRoleRequest(Guid Id, string Name, List<ClaimDto> Claims);

public class UpdateRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/role/update", async ([FromBody] UpdateRoleRequest updateRoleRequest, ISender sender) =>
        {
            var command = updateRoleRequest.Adapt<UpdateRoleCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateRoleResponse>();

            return Results.Ok(response);
        })
        .WithName("UpdateRole")
        .Produces<UpdateRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Update Role");
    }
}