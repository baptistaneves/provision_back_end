namespace ProvisionPadel.Api.Features.Roles.Remove;

public record RemoveRoleResult(bool IsSuccess);

public record RemoveRoleCommand(Guid Id) : ICommand<RemoveRoleResult>;

public class RemoveRoleHandler
    (RoleManager<Role> roleManager, 
    UserManager<User> userManager,
    INotifier notifier) : ICommandHandler<RemoveRoleCommand, RemoveRoleResult>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly INotifier _notifier = notifier;  
    
    public async Task<RemoveRoleResult> Handle(RemoveRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await GetRoleById(command.Id);

        EnsureRoleDoesNotHasUsers(role.Name);

        var result = await roleManager.DeleteAsync(role);
        result.ValidateOperation(_notifier);

        return new RemoveRoleResult(true);
    }

    private async Task<Role> GetRoleById(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());

        if (role is null)
            _notifier.Add("O perfil solicitado não foi encontrado");

        return role;
    }

    private void EnsureRoleDoesNotHasUsers(string name)
    {
        if (userManager.GetUsersInRoleAsync(name).Result.Any())
            _notifier.Add("Este perfil possui usuários associados, não pode ser removido");
    }
}