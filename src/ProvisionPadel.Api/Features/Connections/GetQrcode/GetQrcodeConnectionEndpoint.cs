namespace ProvisionPadel.Api.Features.Connections.GetQrcode;

public class GetQrcodeConnectionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/connection/get-qrcode/{instanceName}", async (string instanceName, IEvolutionApiService evolutionApiService) =>
        {
            var response = await evolutionApiService.InstanceConnect(instanceName);

            return Results.Ok(response);
        })
        .WithName("GetQrcode")
        .Produces<QrcodeDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Qrcode")
        .WithDescription("Get Qrcode");
    }
}