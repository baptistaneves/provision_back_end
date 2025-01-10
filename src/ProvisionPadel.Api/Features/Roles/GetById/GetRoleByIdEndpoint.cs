namespace ProvisionPadel.Api.Features.Roles.GetById;

public record GetRoleByIdResponse(RoleDto Role);

public class GetRoleByIdEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/role/get-role-by-id/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetRoleByIdQuery(id);

            var result = await sender.Send(query);

            return Response(result);
        })
        .WithName("GetRoleById")
        .Produces<GetRoleByIdResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithDescription("Get Role By Id");
    }
}