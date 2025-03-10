﻿namespace ProvisionPadel.Api.Configurations;

public class WebApplicationBuilderConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var assembly = typeof(Program).Assembly;

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddCarter();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        builder.Services.AddValidatorsFromAssembly(assembly);

        var cors = new Cors();
        var evolutionApi = new EvoluctionApi();
        var hikvisionNvr = new HikvisionNVR();
        var ffMpeg = new Fmpeg();
        var bunny = new Bunny();

        builder.Configuration.Bind(nameof(Cors), cors);
        builder.Configuration.Bind(nameof(EvoluctionApi), evolutionApi);
        builder.Configuration.Bind(nameof(HikvisionNVR), hikvisionNvr);
        builder.Configuration.Bind(nameof(Fmpeg), ffMpeg);
        builder.Configuration.Bind(nameof(Bunny), bunny);

        builder.Services.Configure<Cors>(builder.Configuration.GetSection(nameof(Cors)));
        builder.Services.Configure<HikvisionNVR>(builder.Configuration.GetSection(nameof(HikvisionNVR)));
        builder.Services.Configure<EvoluctionApi>(builder.Configuration.GetSection(nameof(EvoluctionApi)));
        builder.Services.Configure<Fmpeg>(builder.Configuration.GetSection(nameof(Fmpeg)));
        builder.Services.Configure<Bunny>(builder.Configuration.GetSection(nameof(Bunny)));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins(cors.ClientUrl)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .WithExposedHeaders("Content-Disposition"));
        });
    }
}