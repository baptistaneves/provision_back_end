using Mapster;

namespace ProvisionPadel.Api.Features.Users.Create;

public record CreateUserResponse(Guid Id, string Token, string Email, string UserName, string PhoneNumber);

public record CreateUserRequest(string Email, string UserName, string PhoneNumber, string Role, string Password);

public class CreateAdminEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/user/create", async ([FromBody] CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateUserResponse>();

            return Results.Ok(response);
        })
        .WithName("CreateUser")
        .Produces<CreateUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Create User");
    }
}