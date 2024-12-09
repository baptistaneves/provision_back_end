using Mapster;

namespace ProvisionPadel.Api.Features.Cameras.StartRecording;

public record StartRecordingResponse(string Content);

public record StartRecordingRequest(int ChannelId);

public class StartRecordingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/nvr/start-recording", async ([FromBody] StartRecordingRequest request, ISender sender) =>
        {
            var command = request.Adapt<StartRecordingCommand>();
            var result = await sender.Send(command);

            var response = result.Adapt<StartRecordingResponse>();

            return Results.Ok(response);
        })
        .WithName("StartRecording")
        .Produces<StartRecordingResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Start Recording")
        .WithDescription("Start Recording");
    }
}