namespace ProvisionPadel.Api.Features.Cameras;

public record CreateCameraRequest(int Channel, Guid CourtId);

public class CreateCameraEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/camera/create", async ([FromBody] CreateCameraRequest request, ICameraService cameraService, CancellationToken cancellationToken) =>
        {
            await cameraService.Create(request.Channel, request.CourtId, cancellationToken);

            return Results.Ok();
        })
        .WithName("CreateCamera")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create New Camera")
        .WithDescription("Create New Camera");
    }
}
