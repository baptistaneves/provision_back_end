namespace ProvisionPadel.Api.Features.Roles.Update;

public record UpdateRoleResult(bool IsSuccess);

public record UpdateRoleCommand(Guid Id, string Name, List<ClaimDto> Claims) : ICommand<UpdateRoleResult>;

public class UpdateRoleHandler
    (RoleManager<Role> roleManager,
    INotifier notifier) : ICommandHandler<UpdateRoleCommand, UpdateRoleResult>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly INotifier _notifier = notifier;

    public async Task<UpdateRoleResult> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await GetRoleById(command.Id);

        await EnsureRoleDoesNotExist(command.Id, command.Name);

        var currentClaims = await roleManager.GetClaimsAsync(role);

        await RemoveClaimFromRole(role, currentClaims, command.Claims);

        await AddClaimToRole(role, currentClaims, command.Claims);

        role.Name = command.Name;

        var result = await roleManager.UpdateAsync(role);

        result.ValidateOperation(_notifier);

        return new UpdateRoleResult(true);
    }

    private async Task EnsureRoleDoesNotExist(Guid id, string roleName)
    {
        if (await roleManager.Roles.Where(x => x.Name == roleName && x.Id != id).AnyAsync())
            _notifier.Add("Já existe um perfil com este nome");
    }

    private async Task<Role> GetRoleById(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());

        if (role is null)
            _notifier.Add("O perfil solicitado não foi encontrado");

        return role;
    }

    private async Task RemoveClaimFromRole(Role role, IList<Claim> currentClaims, List<ClaimDto> newClaims)
    {
        if (!newClaims.Any() || !currentClaims.Any()) return;

        var tasks = newClaims
            .Where(claim => currentClaims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            .Select(async claim =>
            {
                var claimInstance = new Claim(claim.Type, claim.Value);
                var result = await roleManager.RemoveClaimAsync(role, claimInstance);
                result.ValidateOperation(_notifier);
            });

        await Task.WhenAll(tasks);
    }

    private async Task AddClaimToRole(Role role, IList<Claim> currentClaims, List<ClaimDto> newClaims)
    {
        if (!newClaims.Any() || !currentClaims.Any()) return;

        var tasks = newClaims
            .Where(claim => !currentClaims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            .Select(async claim =>
            {
                var claimInstance = new Claim(claim.Type, claim.Value);
                var result = await roleManager.AddClaimAsync(role, claimInstance);
                result.ValidateOperation(_notifier);
            });

        await Task.WhenAll(tasks);
    }
}