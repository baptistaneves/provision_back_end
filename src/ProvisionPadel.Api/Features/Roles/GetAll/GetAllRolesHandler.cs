namespace ProvisionPadel.Api.Features.Roles.GetAll;

public record GetAllRolesResult(IEnumerable<RoleDto> Roles);

public record GetAllRolesQuery : IQuery<GetAllRolesResult>;

public class GetAllRolesHandler(RoleManager<Role> roleManager)
    : IQueryHandler<GetAllRolesQuery, GetAllRolesResult>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    public async Task<GetAllRolesResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var roleDtos = new List<RoleDto>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);

            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                Claims = claims.Select(claim => new ClaimDto(claim.Type, claim.Value)).ToList()
            };

            roleDtos.Add(roleDto);
        }

        return new GetAllRolesResult(roleDtos);
    }
}