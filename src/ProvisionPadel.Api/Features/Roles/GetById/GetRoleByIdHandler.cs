namespace ProvisionPadel.Api.Features.Roles.GetById;

public record GetRoleByIdResult(RoleDto Role);

public record GetRoleByIdQuery(Guid Id) : IQuery<Result<GetRoleByIdResult>>;

public class GetRoleByIdHandler
    (RoleManager<Role> roleManager) : IQueryHandler<GetRoleByIdQuery, Result<GetRoleByIdResult>>
{
    private readonly RoleManager<Role> _roleManager = roleManager;

    public async Task<Result<GetRoleByIdResult>> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(query.Id.ToString());

        if (role is null)
            return Result<GetRoleByIdResult>.Failure(new Error("O perfil solicitado não foi encontrado"));

        var claims = await _roleManager.GetClaimsAsync(role);

        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name!,
            Claims = claims.Select(claim => new ClaimDto(claim.Type, claim.Value)).ToList()
        };

        return Result<GetRoleByIdResult>.Success(new GetRoleByIdResult(roleDto));
    }
}