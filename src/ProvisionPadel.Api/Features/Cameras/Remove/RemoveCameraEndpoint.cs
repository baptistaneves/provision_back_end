namespace ProvisionPadel.Api.Features.Cameras.Remove;

public class RemoveCameraEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/camera/remove/{id}", async (Guid id, ICameraService cameraService, CancellationToken cancellationToken) =>
        {
            var result = await cameraService.Remove(id, cancellationToken);

            return Response(result);
        })
       .WithName("RemoveCamera")
       .Produces(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status404NotFound)
       .WithSummary("Remove Camera")
       .WithDescription("Remove Camera");
    }
}