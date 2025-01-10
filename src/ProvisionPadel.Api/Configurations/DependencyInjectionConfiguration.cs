using ProvisionPadel.Api.Features.Login;

namespace ProvisionPadel.Api.Configurations;

public class DependencyInjectionConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ApplicationDbContext>();

        builder.Services.AddSingleton<HikvisionHttpClient>();

        builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        builder.Services.AddScoped<INotifier, Notifier>();

        builder.Services.AddScoped<IJwtService, JwtService>();

        builder.Services.AddScoped<IHikvisionService, HikvisionService>();
        builder.Services.AddScoped<IVideoService, VideoService>();
        builder.Services.AddScoped<ICameraService, CameraService>();
        builder.Services.AddScoped<IEvolutionApiService, EvolutionApiService>();
        builder.Services.AddScoped<IBunnyService, BunnyService>();

    }
}