namespace ProvisionPadel.Api.Features.Videos.GetAll;

public class GetAllVideosEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/video/get-all", async (IVideoService videoService, CancellationToken cancellationToken) =>
        {
            var response = await videoService.GetAll(cancellationToken);

            return Results.Ok(response);
        })
        .WithName("GetAllVideos")
        .Produces<IEnumerable<Video>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get All Videos")
        .WithDescription("Get All Videos")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Video, Access.View);
        });
    }
}