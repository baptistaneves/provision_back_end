namespace ProvisionPadel.Api.Features.Connections.GetQrcode;

public class GetQrcodeConnectionEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/connection/get-qrcode/{instanceName}", async (string instanceName, IEvolutionApiService evolutionApiService) =>
        {
            var result = await evolutionApiService.InstanceConnect(instanceName);

            return Response(result);
        })
        .WithName("GetQrcode")
        .Produces<Result<QrcodeDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Qrcode")
        .WithDescription("Get Qrcode");
    }
}