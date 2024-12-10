namespace ProvisionPadel.Api.Features.Connections.Create;

public record CreateConnectionRequest(string InstanceName);

public class CreateConnectionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/connection/create", async ([FromBody] CreateConnectionRequest request, IEvolutionApiService evolutionApiService) =>
        {
            var response = await evolutionApiService.CreateInstance(request.InstanceName);

            return Results.Ok(response);
        })
        .WithName("CreateConnection")
        .Produces<bool>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create connection")
        .WithDescription("Create New Connection");
    }
}