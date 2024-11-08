using Mapster;

namespace ProvisionPadel.Api.Features.Roles.GetAll;

public record GetAllRolesResponse(IEnumerable<RoleDto> Roles);

public class GetAllRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/role/get-all", async (ISender sender) =>
        {
            var query = new GetAllRolesQuery();

            var result = await sender.Send(query);

            var response = result.Adapt<GetAllRolesResponse>();

            return Results.Ok(response);
        })
        .WithName("GetAllRoles")
        .Produces<GetAllRolesResponse>(StatusCodes.Status200OK)
        .WithDescription("Get All Roles");
    }
}