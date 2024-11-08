namespace ProvisionPadel.Api.Features.Users.Create;

public record CreateUserResult(Guid Id, string Token, string Email, string UserName, string PhoneNumber);

public record CreateUserCommand(string Email, string UserName, string PhoneNumber, string Role, string Password)
    : ICommand<CreateUserResult>;

public class CreateUserHandler
    (IJwtService jwtService,
     UserManager<User> userManager,
     INotifier notifier) 
    : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly INotifier _notifier = notifier;

    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            UserName = command.UserName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        var userResult = await _userManager.CreateAsync(newUser, command.Password);

        if(userResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, command.Role);
        }

        return new CreateUserResult
        (
            newUser.Id,
            await jwtService.GetJwtString(newUser),
            newUser.Email,
            newUser.UserName,
            newUser.PhoneNumber
        );
    }

}