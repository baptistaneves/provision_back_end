namespace ProvisionPadel.Api.Features.Users.Update;

public record UpdateUserResult(bool IsSuccess);

public record UpdateUserCommand(Guid Id, string UserName, string Email, string PhoneNumber, string Role) 
    : ICommand<UpdateUserResult>;

public class UpdateAdminHandler
    (UserManager<User> userManager,
    INotifier notifier)
    : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly INotifier _notifier = notifier;
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await GetUserById(command.Id);

        user.UserName = command.UserName;
        user.Email = command.Email;
        user.PhoneNumber = command.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        result.ValidateOperation(_notifier);

        if (!String.IsNullOrWhiteSpace(command.Role))
        {
            var currentRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            await RemoveUserFromRole(user, currentRole);

            await AddUserToRole(user, command.Role);
        }

        return new UpdateUserResult(true);
    }

    private async Task<User> GetUserById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            _notifier.Add("Usuário solicitado não foi encontrado");

        return user;
    }

    private async Task RemoveUserFromRole(User user, string role)
    {
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        result.ValidateOperation(_notifier);
    }

    private async Task AddUserToRole(User user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        result.ValidateOperation(_notifier);
    }
}