namespace ProvisionPadel.Api.Features.Connections.GetById;

public class GetConnectionByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/connection/get-by-id/{instanceId}", async (Guid instanceId, IEvolutionApiService evolutionApiService) =>
        {
            var response = await evolutionApiService.FetchInstanceById(instanceId);

            return Results.Ok(response);
        })
        .WithName("GetConnectionById")
        .Produces<InstanceDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get connection by instance id")
        .WithDescription("Get connection by instance id");
    }
}
