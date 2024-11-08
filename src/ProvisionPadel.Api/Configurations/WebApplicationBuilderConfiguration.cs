namespace ProvisionPadel.Api.Configurations;

public class WebApplicationBuilderConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var cors = new Cors();

        builder.Configuration.Bind(nameof(Cors), cors);

        builder.Services.Configure<Cors>(builder.Configuration.GetSection(nameof(Cors)));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins(cors.ClientUrl)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .WithExposedHeaders("Content-Disposition"));
        });

        var apiVersioningBuilder = builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        apiVersioningBuilder.AddVersionedApiExplorer(config =>
        {
            config.GroupNameFormat = "'v'VVV";
            config.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(Program).Assembly); });

        builder.Services.AddCarter();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    }
}