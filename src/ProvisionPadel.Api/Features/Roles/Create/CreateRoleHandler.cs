namespace ProvisionPadel.Api.Features.Roles.Create;

public record CreateRoleResult(bool IsSuccess);

public record CreateRoleCommand(string Name, List<ClaimDto> Claims) : ICommand<CreateRoleResult>;

public class CreateRoleHandler
    (RoleManager<Role> roleManager,
    INotifier notifier) : ICommandHandler<CreateRoleCommand, CreateRoleResult>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly INotifier _notifier = notifier;

    public async Task<CreateRoleResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        await EnsureRoleDoesNotExist(command.Name);

        var newRole = new Role { Name = command.Name };

        var result = await _roleManager.CreateAsync(newRole);

        result.ValidateOperation(_notifier);

        await AddClaimsToRole(newRole, command.Claims);

        return new CreateRoleResult(true);
    }

    private async Task EnsureRoleDoesNotExist(string roleName)
    {
        if (await _roleManager.Roles.Where(x => x.Name == roleName).AnyAsync())
            _notifier.Add("Já existe uma role com este nome");
    }

    private async Task AddClaimsToRole(Role role, List<ClaimDto> claims)
    {
        if (!claims.Any()) return;

        foreach (var claim in claims)
        {
            var result = await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));

            result.ValidateOperation(_notifier);
        }
    }
}