namespace ProvisionPadel.Api.Features.Courts.Remove;

public record RemoveCourtResponse(bool IsSuccess);

public class RemoveCourtEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/court/remove/{id}", async (Guid id, ISender sender) =>
        {
            var command = new RemoveCourtCommand(id);

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("RemoveCourte")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Remove Court")
        .WithDescription("Remove Court");
    }
}
