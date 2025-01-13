using Mapster;

namespace ProvisionPadel.Api.Features.Courts.Create;

public record CreateCourtRequest(string Description);

public record CreateCourtResponse(bool IsSuccess);

public class CreateCourtEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/court/create", async ([FromBody] CreateCourtRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateCourtCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("CreateCourt")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Create Court")
        .WithDescription("Create Court")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Court, Access.ManageCourt);
        });
    }
}