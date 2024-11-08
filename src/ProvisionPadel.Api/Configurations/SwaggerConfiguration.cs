namespace ProvisionPadel.Api.Configurations;

public class SwaggerConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}
