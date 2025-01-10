namespace ProvisionPadel.Api.Features.Users.Create;

public record CreateUserResult(Guid Id, string Token, string Email, string UserName, string PhoneNumber);

public record CreateUserCommand(string Email, string UserName, string PhoneNumber, string Role, string Password)
    : ICommand<Result<CreateUserResult>>;

public class CreateUserHandler
    (IJwtService jwtService,
     UserManager<User> userManager) 
    : ICommandHandler<CreateUserCommand, Result<CreateUserResult>>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Result<CreateUserResult>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            UserName = command.UserName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        var userResult = await _userManager.CreateAsync(newUser, command.Password);

        if(!userResult.Succeeded)
        {
            var errors = userResult.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<CreateUserResult>.Failure(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(newUser, command.Role);

        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<CreateUserResult>.Failure(errors);
        }

        return Result<CreateUserResult>.Success(new CreateUserResult
        (
            newUser.Id,
            await jwtService.GetJwtString(newUser),
            newUser.Email,
            newUser.UserName,
            newUser.PhoneNumber
        ));
    }

}