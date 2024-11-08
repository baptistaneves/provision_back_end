namespace ProvisionPadel.Api.Configurations;

public class DbConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
    }
}