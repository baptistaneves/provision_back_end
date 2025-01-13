using Mapster;

namespace ProvisionPadel.Api.Features.Roles.Create;

public record CreateRoleRequest(string Name, List<ClaimDto> Claims);

public class CreateRoleEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/role/create", async ([FromBody] CreateRoleRequest createRoleRequest, ISender sender) =>
        {
            var command = createRoleRequest.Adapt<CreateRoleCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("CreateRole")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Create Role")
        .RequireAuthorization(options =>
        {
            options.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            options.RequireClaim(Access.Role, Access.ManageRole);
        });
    }
}