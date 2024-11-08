namespace ProvisionPadel.Api.Configurations;

public class IdentityConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.User.AllowedUserNameCharacters = string.Empty;
        })
        .AddErrorDescriber<IdentityPortugueseMessages>()
        .AddUserManager<UserManager<User>>()
        .AddRoleManager<RoleManager<Role>>()
        .AddSignInManager<SignInManager<User>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }
}
