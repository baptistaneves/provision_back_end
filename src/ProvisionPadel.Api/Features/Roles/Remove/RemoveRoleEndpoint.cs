namespace ProvisionPadel.Api.Features.Roles.Remove;

public class RemoveRoleEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/role/remove/{id}", async (Guid id, ISender sender) =>
        {
            var command = new RemoveRoleCommand(id);

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("RemoveRole")
        .Produces<Result<bool>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithDescription("Remove Role");
    }
}