namespace ProvisionPadel.Api.Features.Roles.Update;

public record UpdateRoleCommand(Guid Id, string Name, List<ClaimDto> Claims) : ICommand<Result<bool>>;

public class UpdateRoleHandler
    (RoleManager<Role> roleManager,
    INotifier notifier) : ICommandHandler<UpdateRoleCommand, Result<bool>>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly INotifier _notifier = notifier;

    public async Task<Result<bool>> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(command.Id.ToString());

        if (role is null)
            return Result<bool>.Failure(new Error("O perfil solicitado não foi encontrado"));

        if (await roleManager.Roles.Where(x => x.Name == command.Name && x.Id != command.Id).AnyAsync())
            return Result<bool>.Failure(new Error("Já existe um perfil com este nome"));

        var currentClaims = await roleManager.GetClaimsAsync(role);

        await RemoveClaimFromRole(role, currentClaims, command.Claims);

        await AddClaimToRole(role, currentClaims, command.Claims);

        role.Name = command.Name;

        var result = await roleManager.UpdateAsync(role);

        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }

    private async Task RemoveClaimFromRole(Role role, IList<Claim> currentClaims, List<ClaimDto> newClaims)
    {
        if (!newClaims.Any() || !currentClaims.Any()) return;

        var tasks = newClaims
            .Where(claim => currentClaims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            .Select(async claim =>
            {
                var claimInstance = new Claim(claim.Type, claim.Value);
                await roleManager.RemoveClaimAsync(role, claimInstance);
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
                await roleManager.AddClaimAsync(role, claimInstance);
            });

        await Task.WhenAll(tasks);
    }
}