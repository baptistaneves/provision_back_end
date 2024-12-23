namespace ProvisionPadel.Api.Features.Cameras.Remove;

public class RemoveCameraEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/camera/remove/{id}", async (Guid id, ICameraService cameraService, CancellationToken cancellationToken) =>
        {
            var response = await cameraService.Remove(id, cancellationToken);

            return Results.Ok(response);
        })
       .WithName("RemoveCamera")
       .Produces(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status404NotFound)
       .WithSummary("Remove Camera")
       .WithDescription("Remove Camera");
    }
}