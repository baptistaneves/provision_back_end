using Carter;

namespace ProvisionPadel.Api.Configurations;

public class ApplicationConfiguration : IWebApplicationRegister
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        app.UseCors("AllowOrigin");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapCarter();
    }
}
