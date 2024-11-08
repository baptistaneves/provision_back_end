using Mapster;

namespace ProvisionPadel.Api.Features.Users.Update;

public record UpdateUserResponse(bool IsSuccess);

public record UpdateUserRequest(Guid Id, string UserName, string Email, string PhoneNumber, string Role);

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/update", async ([FromBody] UpdateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateUserCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateUserResponse>();

            return Results.Ok(response);
        })
        .WithName("UpdateUser")
        .Produces<UpdateUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Update User");
    }
}