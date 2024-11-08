﻿namespace ProvisionPadel.Api.Features.Cameras;

public class GetAllCamerasEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/camera/get-all", async (ICameraService cameraService, CancellationToken cancellationToken) =>
        {
            var response = await cameraService.GetAll(cancellationToken);

            return Results.Ok(response);
        })
        .WithName("GetAllCameras")
        .Produces<IEnumerable<Camera>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get All Cameras")
        .WithDescription("Get All Cameras");
    }
}
