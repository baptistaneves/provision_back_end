namespace ProvisionPadel.Api.Options;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }

        var scheme = GetJwtSecurityScheme();
        options.AddSecurityDefinition(scheme.Reference.Id, scheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {scheme, new string[0] }
        });
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Sistema de gravação e gestão de vídeos de jogos de padel",
            Version = description.ApiVersion.ToString(),
            Description = "O Provision Padel é um sistema projetado para automatizar a gravação e entrega de vídeos das partidas." +
            "O sistema integra câmeras Hikvision e o NVR via API.",
            Contact = new OpenApiContact
            {
                Name = "Provision Padel",
                Email = "contacto@ucall.ao"
            },
            License = new OpenApiLicense
            {
                Name = "CC BY"
            }
        };

        if (description.IsDeprecated)
        {
            info.Description = "Essa versão da API está absoleta";

        }
        return info;
    }

    private OpenApiSecurityScheme GetJwtSecurityScheme()
    {
        return new OpenApiSecurityScheme
        {
            Name = "Autenticação JWT",
            Description = "Copie 'Bearer ' + token'",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
    }

}
