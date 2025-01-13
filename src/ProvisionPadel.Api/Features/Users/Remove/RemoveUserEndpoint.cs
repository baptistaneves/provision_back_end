namespace ProvisionPadel.Api.Features.Users.Remove;

public class RemoveUserEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user/remove/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveUserCommand(id));

            return Response(result);
        })
         .WithName("RemoveUser")
         .Produces<Result<bool>>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Remove User")
         .RequireAuthorization(options =>
         {
             options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
             options.RequireClaim(Access.User, Access.ManageUser);
         });
    }
}
