namespace ProvisionPadel.Api.Features.Connections.GetById;

public class GetConnectionByIdEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/connection/get-by-id/{instanceId}", async (Guid instanceId, IEvolutionApiService evolutionApiService) =>
        {
            var result = await evolutionApiService.FetchInstanceById(instanceId);

            return Response(result);
        })
        .WithName("GetConnectionById")
        .Produces<Result<InstanceDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get connection by instance id")
        .WithDescription("Get connection by instance id")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Connection, Access.View);
        });
    }
}
