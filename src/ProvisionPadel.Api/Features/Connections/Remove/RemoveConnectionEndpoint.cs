namespace ProvisionPadel.Api.Features.Connections.Remove;

public class RemoveConnectionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/connection/remove/{instanceName}", async (string instanceName, IEvolutionApiService evolutionApiService) =>
        {
            var response = await evolutionApiService.DeleteInstance(instanceName);

            return Results.Ok(response);
        })
        .WithName("RemoveConnection")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Remove connection")
        .WithDescription("Remove Connection");
    }
}