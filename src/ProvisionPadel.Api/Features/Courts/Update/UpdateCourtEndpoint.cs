using Mapster;

namespace ProvisionPadel.Api.Features.Courts.Update;

public record UpdateCourtRequest(Guid Id, string Description);

public class UpdateCourtEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/court/update", async ([FromBody] UpdateCourtRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateCourtCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("Updatecourt")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Court")
        .WithDescription("Update Court")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Court, Access.ManageCourt);
        });
    }
}