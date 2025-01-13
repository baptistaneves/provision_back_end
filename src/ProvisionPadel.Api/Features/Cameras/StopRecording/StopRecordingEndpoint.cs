using Mapster;

namespace ProvisionPadel.Api.Features.Cameras.StopRecording;

public record StopRecordingRequest(int ChannelId);

public class StopRecordingEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/nvr/stop-recording", async ([FromBody] StopRecordingRequest request, ISender sender) =>
        {
            var command = request.Adapt<StopRecordingCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("StopRecording")
        .Produces<Result<string>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Stop Recording")
        .WithDescription("Stop Recording")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Camera, Access.StopRecording);
        });
    }
}