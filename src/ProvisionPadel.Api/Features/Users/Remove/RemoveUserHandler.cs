namespace ProvisionPadel.Api.Features.Users.Remove;

public record RemoveUserCommand(Guid Id) : ICommand<Result<bool>>;

public class RemoveAdminHandler
    (UserManager<User> userManager,
    INotifier notifier)
    : ICommandHandler<RemoveUserCommand, Result<bool>>
{
    private readonly INotifier _notifier = notifier;
    private readonly UserManager<User> _userManager = userManager;
    public async Task<Result<bool>> Handle(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.Id.ToString());

        if (user is null)
            return Result<bool>.Failure(new Error("O usuário solicitado não foi encontrado"));

        var result = await _userManager.DeleteAsync(user);

        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }

}