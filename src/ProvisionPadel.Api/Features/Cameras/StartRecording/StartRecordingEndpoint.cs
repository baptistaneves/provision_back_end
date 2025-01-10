using Mapster;

namespace ProvisionPadel.Api.Features.Cameras.StartRecording;

public record StartRecordingRequest(int ChannelId);

public class StartRecordingEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/nvr/start-recording", async ([FromBody] StartRecordingRequest request, ISender sender) =>
        {
            var command = request.Adapt<StartRecordingCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("StartRecording")
        .Produces<Result<string>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Start Recording")
        .WithDescription("Start Recording");
    }
}