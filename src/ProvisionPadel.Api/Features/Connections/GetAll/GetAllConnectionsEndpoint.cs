namespace ProvisionPadel.Api.Features.Connections.GetAll;

public class GetAllConnectionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/connection/get-all", async (IEvolutionApiService evolutionApiService) =>
        {
            var response = await evolutionApiService.FetchInstances();

            return Results.Ok(response);
        })
        .WithName("GetAllConnections")
        .Produces<IEnumerable<InstanceDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get all connections")
        .WithDescription("Get all connections")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Connection, Access.View);
        });
    }
}
