using Carter;

namespace ProvisionPadel.Api.Configurations;

public class ApplicationConfiguration : IWebApplicationRegister
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        app.UseCors("AllowOrigin");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.ApiVersion.ToString());
            }
        });

        app.MapCarter();
    }
}
