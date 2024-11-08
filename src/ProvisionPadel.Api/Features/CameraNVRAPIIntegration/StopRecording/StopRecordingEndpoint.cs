using Mapster;

namespace ProvisionPadel.Api.Features.CameraNVRAPIIntegration.StopRecording;

public record StopRecordingRequest(int ChannelId);

public record StopRecordingResponse(string Content);

public class StopRecordingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/nvr/stop-recording", async ([FromBody] StopRecordingRequest request, ISender sender) =>
        {
            var command = request.Adapt<StopRecordingCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<StopRecordingResponse>();

            return Results.Ok(response);
        })
        .WithName("StopRecording")
        .Produces<StopRecordingResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Stop Recording")
        .WithDescription("Stop Recording");
    }
}