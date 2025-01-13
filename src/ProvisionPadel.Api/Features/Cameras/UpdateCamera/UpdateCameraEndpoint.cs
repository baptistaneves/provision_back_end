using ProvisionPadel.Api.Shared.Accesses;

namespace ProvisionPadel.Api.Features.Cameras.UpdateCamera;

public class UpdateCameraEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/camera/update", async ([FromBody] UpdateCameraRequest request, ICameraService cameraService, CancellationToken cancellationToken) =>
        {
            var result = await cameraService.Update(request, cancellationToken);

            return Response(result);
        })
        .WithName("UpdateCamera")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Camera")
        .WithDescription("Update Camera")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Camera, Access.ManageCamera);
        });
    }
}
