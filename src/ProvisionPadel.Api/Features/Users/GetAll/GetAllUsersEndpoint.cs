using Mapster;

namespace ProvisionPadel.Api.Features.Users.GetAll;

public record GetAllUsersResponse(IEnumerable<User> Users);

public class GetAllUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-all", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllUsersQuery());

            var response = result.Adapt<GetAllUsersResponse>();

            return Results.Ok(response);
        })
        .WithName("GetAllUsers")
        .Produces<GetAllUsersResponse>(StatusCodes.Status200OK)
        .WithDescription("Get All Users")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.User, Access.View);
        });
    }
}
