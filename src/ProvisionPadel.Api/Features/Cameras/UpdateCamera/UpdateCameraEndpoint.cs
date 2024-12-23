namespace ProvisionPadel.Api.Features.Cameras.UpdateCamera;

public record UpdateCameraRequest(Guid Id, int Channel, Guid CourtId);

public class UpdateCameraEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/camera/update", async ([FromBody] UpdateCameraRequest request, ICameraService cameraService, CancellationToken cancellationToken) =>
        {
            var response = await cameraService.Update(request.Id, request.Channel, request.CourtId, cancellationToken);

            return Results.Ok(response);
        })
        .WithName("UpdateCamera")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Camera")
        .WithDescription("Update Camera");
    }
}
