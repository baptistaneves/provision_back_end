namespace ProvisionPadel.Api.Features.Users.Remove;

public record RemoveUserResult(bool IsSuccess);

public record RemoveUserCommand(Guid Id) : ICommand<RemoveUserResult>;

public class RemoveAdminHandler
    (UserManager<User> userManager,
    INotifier notifier)
    : ICommandHandler<RemoveUserCommand, RemoveUserResult>
{
    private readonly INotifier _notifier = notifier;
    private readonly UserManager<User> _userManager = userManager;
    public async Task<RemoveUserResult> Handle(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        var user = await GetUserById(command.Id);

        var result = await _userManager.DeleteAsync(user);

        result.ValidateOperation(_notifier);

        return new RemoveUserResult(true);
    }

    private async Task<User> GetUserById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
            _notifier.Add("O usuário solicitado não foi encontrado");

        return user;
    }

}