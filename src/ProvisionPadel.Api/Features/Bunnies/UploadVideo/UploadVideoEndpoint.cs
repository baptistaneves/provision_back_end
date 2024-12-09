using Mapster;

namespace ProvisionPadel.Api.Features.Bunnies.UploadVideo;

public record UploadVideoResponse(bool IsSuccess);

public record UploadVideoRequest(string VideoName, Guid LibraryId);

public class UploadVideoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/video/upload-video", async ([FromBody] UploadVideoRequest request, ISender sender) =>
        {
            var command = request.Adapt<UploadVideoCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UploadVideoResponse>();

            return Results.Ok(response);
        })
        .WithName("UploadVideo")
        .Produces<UploadVideoResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Upload Video")
        .WithDescription("Upload new video");
    }
}