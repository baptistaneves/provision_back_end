namespace ProvisionPadel.Api.Services.Interfaces;

public interface IJwtService
{
    Task<string> GetJwtString(User user);
}
