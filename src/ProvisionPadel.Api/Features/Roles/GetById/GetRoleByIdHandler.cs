namespace ProvisionPadel.Api.Features.Roles.GetById;

public record GetRoleByIdResult(RoleDto Role);

public record GetRoleByIdQuery(Guid Id) : IQuery<GetRoleByIdResult>;

public class GetRoleByIdHandler
    (RoleManager<Role> roleManager,
    INotifier notifier) : IQueryHandler<GetRoleByIdQuery, GetRoleByIdResult>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly INotifier _notifier = notifier;
    public async Task<GetRoleByIdResult> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(query.Id.ToString());
        if (role is null)
        {
            _notifier.Add("O perfil solicitado não foi encontrado");
            return null;
        }

        var claims = await _roleManager.GetClaimsAsync(role);

        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Claims = claims.Select(claim => new ClaimDto(claim.Type, claim.Value)).ToList()
        };

        return new GetRoleByIdResult(roleDto);
    }
}