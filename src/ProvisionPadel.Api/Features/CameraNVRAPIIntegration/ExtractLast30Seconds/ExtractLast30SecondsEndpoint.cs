namespace ProvisionPadel.Api.Features.CameraNVRAPIIntegration.ExtractLast30Seconds;

public class ExtractLast30SecondsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/video/extract-last-30-seconds/{fileName}", async (string fileName, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new ExtractLast30SecondsCommand(fileName);

            var result = await sender.Send(command, cancellationToken);

            return Results.Ok(result);
        })
        .WithName("ExtractLast30Seconds")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Extract Last 30 Seconds")
        .WithDescription("Extract The Last 30 Seconds of the Video.");
    }

    
}