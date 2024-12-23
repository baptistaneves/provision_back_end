using Mapster;

namespace ProvisionPadel.Api.Features.Courts.Remove;

public record RemoveCourtResponse(bool IsSuccess);

public class RemoveCourtEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/court/remove/{id}", async (Guid id, ISender sender) =>
        {
            var command = new RemoveCourtCommand(id);

            var result = await sender.Send(command);

            var response = result.Adapt<RemoveCourtResponse>();

            return Results.Ok(response);
        })
        .WithName("RemoveCourte")
        .Produces<RemoveCourtResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Remove Court")
        .WithDescription("Remove Court");
    }
}
