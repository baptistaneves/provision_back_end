namespace ProvisionPadel.Api.Features.Videos.GetVideo;

public class GetVideoEndpoint(IConfiguration configuration) : ICarterModule
{
    private readonly string _videoDirectory = configuration["FFmpeg:VideoDirectory"];

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/video/get-video/{fileName}", async (string fileName, CancellationToken cancellationToken) =>
        {
            var filePath = Path.Combine(_videoDirectory, fileName + ".mp4");

            if (!System.IO.File.Exists(filePath))
            {
                return Results.NotFound($"Video file '{fileName}' not found.");
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return Results.Stream(stream, "video/mp4");
        })
        .WithName("GetVideo")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Get Video")
        .WithDescription("Stream the video file in response.");
    }

}