using Mapster;

namespace ProvisionPadel.Api.Features.Users.Update;

public record UpdateUserRequest(Guid Id, string UserName, string Email, string PhoneNumber, string Role);

public class UpdateUserEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/update", async ([FromBody] UpdateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateUserCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("UpdateUser")
        .Produces<Result<bool>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Update User")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.User, Access.ManageUser);
        });
    }
}