using Mapster;

namespace ProvisionPadel.Api.Features.Bunnies.UploadVideo;

public record UploadVideoRequest(int ChannelId, string Name, string Size, DateTime StartTime, DateTime EndTime);

public class UploadVideoEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/video/upload-video", async ([FromBody] UploadVideoRequest request, ISender sender) =>
        {
            var command = request.Adapt<UploadVideoCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("UploadVideo")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Upload Video")
        .WithDescription("Upload new video")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Camera, Access.ManageCamera);
        });
    }
}