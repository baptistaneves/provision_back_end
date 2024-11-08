namespace ProvisionPadel.Api.Configurations.Interfaces;

public interface IWebApplicationBuilderRegister : IRegister
{
    void RegisterServices(WebApplicationBuilder builder);
}
