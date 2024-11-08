using Mapster;

namespace ProvisionPadel.Api.Features.Users.Remove;

public record RemoveUserResponse(bool IsSuccess);

public class RemoveUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user/remove/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveUserCommand(id));

            var response = result.Adapt<RemoveUserResponse>();

            return Results.Ok(response);
        })
         .WithName("RemoveUser")
         .Produces<RemoveUserResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Remove User");
    }
}
