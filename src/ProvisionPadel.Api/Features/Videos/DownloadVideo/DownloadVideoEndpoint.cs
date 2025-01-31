using Mapster;

namespace ProvisionPadel.Api.Features.Videos.DownloadVideo;

public record DownloadVideoRequest(int ChannelId, string Name, string Size, DateTime StartTime, DateTime EndTime);

public record DownloadVideoResponse(byte[] Content);

public class DownloadVideoEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/nvr/download-video", async ([FromBody] DownloadVideoRequest request, ISender sender) =>
        {
            var command = request.Adapt<DownloadVideoCommand>();

            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            var fileResult = Results.File(
               fileContents: result.Value!,
               contentType: "video/mp4",
               fileDownloadName: $"{request.Name}.mp4"
            );

            return fileResult;
        })
        .WithName("DownloadVideo")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Download Video")
        .WithDescription("Download Video")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Video, Access.View);
        });
    }
}