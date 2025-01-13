using ProvisionPadel.Api.Shared.Accesses;

namespace ProvisionPadel.Api.Features.Cameras.CreateCamera;

public class CreateCameraEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/camera/create", async ([FromBody] CreateCameraRequest request, ICameraService cameraService, CancellationToken cancellationToken) =>
        {
            var result = await cameraService.Create(request, cancellationToken);

            return Response(result);
        })
        .WithName("CreateCamera")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create New Camera")
        .WithDescription("Create New Camera")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Camera, Access.ManageCamera);
        });
    }
}