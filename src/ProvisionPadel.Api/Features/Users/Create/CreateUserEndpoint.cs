using Mapster;

namespace ProvisionPadel.Api.Features.Users.Create;

public record CreateUserRequest(string Email, string UserName, string PhoneNumber, string Role, string Password);

public class CreateAdminEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/user/create", async ([FromBody] CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("CreateUser")
        .Produces<Result<CreateUserResult>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Create User")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.User, Access.ManageUser);
        });
    }
}