namespace ProvisionPadel.Api.Features.Users.Update;

public record UpdateUserCommand(Guid Id, string UserName, string Email, string PhoneNumber, string Role) 
    : ICommand<Result<bool>>;

public class UpdateAdminHandler
    (UserManager<User> userManager,
    INotifier notifier)
    : ICommandHandler<UpdateUserCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly INotifier _notifier = notifier;
    public async Task<Result<bool>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.Id.ToString());

        if (user is null)
            return Result<bool>.Failure(new Error("Usuário solicitado não foi encontrado"));

        user.UserName = command.UserName;
        user.Email = command.Email;
        user.PhoneNumber = command.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<bool>.Failure(errors);
        }

        if (!String.IsNullOrWhiteSpace(command.Role))
        {
            var currentRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            await _userManager.RemoveFromRoleAsync(user, currentRole);

            await _userManager.AddToRoleAsync(user, command.Role);
        }

        return Result<bool>.Success(true);
    }
}