namespace ProvisionPadel.Api.Features.Roles.Create;

public record CreateRoleCommand(string Name, List<ClaimDto> Claims) : ICommand<Result<bool>>;

public class CreateRoleHandler
    (RoleManager<Role> roleManager,
    INotifier notifier) : ICommandHandler<CreateRoleCommand, Result<bool>>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly INotifier _notifier = notifier;

    public async Task<Result<bool>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        if(await EnsureRoleDoesNotExist(command.Name))
            return Result<bool>.Failure(new Error("Já existe um perfil com este nome"));

        var newRole = new Role { Name = command.Name };

        var result = await _roleManager.CreateAsync(newRole);

        if(!result.Succeeded )
        {
            var errors = result.Errors.Select(error => new Error(error.Description)).ToList();

            return Result<bool>.Failure(errors);
        }

        if(command.Claims.Any())
        {
            foreach(var claim in command.Claims)
            {
                await _roleManager.AddClaimAsync(newRole, new Claim(claim.Type, claim.Value));
            }
        }

        return Result<bool>.Success(true);
    }

    private async Task<bool>EnsureRoleDoesNotExist(string roleName)
    {
        return await _roleManager.Roles.Where(x => x.Name == roleName).AnyAsync();
    }
}