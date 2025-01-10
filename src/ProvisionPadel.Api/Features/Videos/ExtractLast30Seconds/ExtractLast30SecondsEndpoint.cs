using Mapster;

namespace ProvisionPadel.Api.Features.Videos.ExtractLast30Seconds;

public record ExtractLast30SecondsRequest(string FileName, string InstanceName, string Destination);

public class ExtractLast30SecondsEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/video/extract-last-30-seconds", async ([FromBody] ExtractLast30SecondsRequest request, ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<ExtractLast30SecondsCommand>();

            var response = await sender.Send(command, cancellationToken);

            return Response(response);
        })
        .WithName("ExtractLast30Seconds")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Extract Last 30 Seconds")
        .WithDescription("Extract The Last 30 Seconds of the Video.");
    }


}