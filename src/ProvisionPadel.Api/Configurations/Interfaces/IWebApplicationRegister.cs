namespace ProvisionPadel.Api.Configurations.Interfaces;

public interface IWebApplicationRegister : IRegister
{
    void RegisterPipelineComponents(WebApplication app);
}
