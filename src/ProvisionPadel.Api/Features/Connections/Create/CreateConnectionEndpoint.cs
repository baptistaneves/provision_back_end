namespace ProvisionPadel.Api.Features.Connections.Create;

public record CreateConnectionRequest(string InstanceName);

public class CreateConnectionEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/connection/create", async ([FromBody] CreateConnectionRequest request, IEvolutionApiService evolutionApiService) =>
        {
            var result = await evolutionApiService.CreateInstance(request.InstanceName);

            return Response(result);
        })
        .WithName("CreateConnection")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create connection")
        .WithDescription("Create New Connection");
    }
}