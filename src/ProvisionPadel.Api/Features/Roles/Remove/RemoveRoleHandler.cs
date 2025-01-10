namespace ProvisionPadel.Api.Features.Roles.Remove;


public record RemoveRoleCommand(Guid Id) : ICommand<Result<bool>>;

public class RemoveRoleHandler
    (RoleManager<Role> roleManager, 
    UserManager<User> userManager) : ICommandHandler<RemoveRoleCommand, Result<bool>>
{
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly UserManager<User> _userManager = userManager;  
    
    public async Task<Result<bool>> Handle(RemoveRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(command.Id.ToString());

        if (role is null)
           return Result<bool>.Failure(new Error("O perfil solicitado não foi encontrado"));

        if (_userManager.GetUsersInRoleAsync(role.Name!).Result.Any())
            return Result<bool>.Failure(new Error("Este perfil possui usuários associados, não pode ser removido"));

        var result = await _roleManager.DeleteAsync(role);

        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new Error(error.Description)).ToList();

            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }
}