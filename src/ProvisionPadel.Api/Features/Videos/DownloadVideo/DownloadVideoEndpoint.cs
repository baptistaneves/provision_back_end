using Mapster;

namespace ProvisionPadel.Api.Features.Videos.DownloadVideo;

public record DownloadVideoRequest(int ChannelId, string Name, string Size, DateTime StartTime, DateTime EndTime);

public record DownloadVideoResponse(byte[] Content);

public class DownloadVideoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/nvr/download-video", async ([FromBody] DownloadVideoRequest request, ISender sender) =>
        {
            var command = request.Adapt<DownloadVideoCommand>();

            var result = await sender.Send(command);

            var videoStream = result.Adapt<DownloadVideoResponse>();

            var fileResult = Results.File(
               fileContents: videoStream.Content,
               contentType: "video/mp4",
               fileDownloadName: $"{request.Name}.mp4"
            );

            return fileResult;
        })
        .WithName("DownloadVideo")
        .Produces<DownloadVideoResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Download Video")
        .WithDescription("Download Video");
    }
}