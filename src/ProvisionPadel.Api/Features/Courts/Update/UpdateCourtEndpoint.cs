using Mapster;

namespace ProvisionPadel.Api.Features.Courts.Update;

public record UpdateCourtRequest(Guid Id, string Description);

public record UpdateCourtResponse(bool IsSuccess);

public class UpdateCourtEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/court/update", async ([FromBody] UpdateCourtRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateCourtCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateCourtResponse>();

            return Results.Ok(response);
        })
        .WithName("Updatecourt")
        .Produces<UpdateCourtResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Court")
        .WithDescription("Update Court");
    }
}
