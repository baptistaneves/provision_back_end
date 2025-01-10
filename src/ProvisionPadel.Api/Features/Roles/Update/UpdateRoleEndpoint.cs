using Mapster;

namespace ProvisionPadel.Api.Features.Roles.Update;

public record UpdateRoleRequest(Guid Id, string Name, List<ClaimDto> Claims);

public class UpdateRoleEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/role/update", async ([FromBody] UpdateRoleRequest updateRoleRequest, ISender sender) =>
        {
            var command = updateRoleRequest.Adapt<UpdateRoleCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("UpdateRole")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Update Role");
    }
}