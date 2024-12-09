namespace ProvisionPadel.Api.Features.Videos.GetVideo;

public class GetVideoEndpoint(IOptions<Fmpeg> fmpeg) : ICarterModule
{
    private readonly Fmpeg _ffmpeg = fmpeg.Value;

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/video/get-video/{fileName}", async (string fileName, CancellationToken cancellationToken) =>
        {
            var filePath = Path.Combine(_ffmpeg.VideoDirectory, fileName + ".mp4");

            if (!File.Exists(filePath))
            {
                return Results.NotFound($"Video file '{fileName}' not found.");
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return Results.Stream(stream, "video/mp4", enableRangeProcessing: true);
        })
        .WithName("GetVideo")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Get Video")
        .WithDescription("Stream the video file in response.");
    }

}