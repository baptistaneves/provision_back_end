namespace ProvisionPadel.Api.Features.Connections.Remove;

public class RemoveConnectionEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/connection/remove/{instanceName}", async (string instanceName, IEvolutionApiService evolutionApiService) =>
        {
            var result = await evolutionApiService.DeleteInstance(instanceName);

            return Response(result);
        })
        .WithName("RemoveConnection")
        .Produces<Result<bool>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Remove connection")
        .WithDescription("Remove Connection");
    }
}