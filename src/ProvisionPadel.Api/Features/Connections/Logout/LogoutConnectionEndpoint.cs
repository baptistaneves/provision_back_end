namespace ProvisionPadel.Api.Features.Connections.Logout;

public class LogoutConnectionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/connection/logout/{instanceName}", async (string instanceName, IEvolutionApiService evolutionApiService) =>
        {
            var response = await evolutionApiService.LogoutInstance(instanceName);

            return Results.Ok(response);
        })
        .WithName("LogoutConnection")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Logout Connection")
        .WithDescription("Logout Connection");
    }
}