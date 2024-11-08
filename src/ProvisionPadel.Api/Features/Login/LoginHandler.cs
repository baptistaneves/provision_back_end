namespace ProvisionPadel.Api.Features.Login;

public record LoginResult(Guid Id, string Token, string Email, string Name);

public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;

public class LoginHandler
    (SignInManager<User> signInManager,
     UserManager<User> userManager,
     IJwtService jwtService,
     INotifier notifier)
    : ICommandHandler<LoginCommand, LoginResult>
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IJwtService _jwtService = jwtService;
    private readonly INotifier _notifier = notifier;

    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            _notifier.Add("Nome de utilizador ou senha errada");
            return null;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, true);

        if (signInResult.IsLockedOut)
        {
            _notifier.Add("Usuário temporariamente bloqueado");
            return null;
        }

        if (!signInResult.Succeeded)
        {
            _notifier.Add("Nome de utilizador ou senha errada");
            return null;
        }

        return new LoginResult
        (
            user.Id,
            await _jwtService.GetJwtString(user),
            user.Email,
            user.UserName
        );
    }

}
