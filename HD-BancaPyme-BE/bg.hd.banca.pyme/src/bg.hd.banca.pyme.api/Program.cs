using bg.hd.banca.pyme.application.ioc;
using bg.hd.banca.pyme.api.Extentions;
using bg.hd.banca.pyme.infrastructure.extentions;
using bg.hd.banca.pyme.infrastructure.ioc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using bg.hd.banca.pyme.infrastructure.seguridad;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);
//// Add services to the container.

IConfigurationSection myArraySection = builder.Configuration.GetSection("AuthorizeSite:SiteUrl");
            string[] corsURL = myArraySection.Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("myAllowSpecificOrigins",
    builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .WithOrigins(corsURL)
               .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<RequiredHeaderParameter>();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = builder.Configuration["OpenApi:info:title"],
        Description = builder.Configuration["OpenApi:info:description"],
        TermsOfService = new Uri(builder.Configuration["OpenApi:info:termsOfService"]),
        Contact = new OpenApiContact
        {
            Name = builder.Configuration["OpenApi:info:contact:name"],
            Url = new Uri(builder.Configuration["OpenApi:info:contact:url"]),
            Email = builder.Configuration["OpenApi:info:contact:email"]
        },
        License = new OpenApiLicense
        {
            Name = builder.Configuration["OpenApi:info:License:name"],
            Url = new Uri(builder.Configuration["OpenApi:info:License:url"])
        }
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = builder.Configuration["OpenApi:info:title"],
        Description = builder.Configuration["OpenApi:info:description"],
        TermsOfService = new Uri(builder.Configuration["OpenApi:info:termsOfService"]),
        Contact = new OpenApiContact
        {
            Name = builder.Configuration["OpenApi:info:contact:name"],
            Url = new Uri(builder.Configuration["OpenApi:info:contact:url"]),
            Email = builder.Configuration["OpenApi:info:contact:email"]
        },
        License = new OpenApiLicense
        {
            Name = builder.Configuration["OpenApi:info:License:name"],
            Url = new Uri(builder.Configuration["OpenApi:info:License:url"])
        }
    });

    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
    //options.SwaggerDoc(builder.Configuration["OpenApi:info:version"], new OpenApiInfo
    //{
    //    Version = builder.Configuration["OpenApi:info:version"],
    //    Title = builder.Configuration["OpenApi:info:title"],
    //    Description = builder.Configuration["OpenApi:info:description"],
    //    TermsOfService = new Uri(builder.Configuration["OpenApi:info:termsOfService"]),
    //    Contact = new OpenApiContact
    //    {
    //        Name = builder.Configuration["OpenApi:info:contact:name"],
    //        Url = new Uri(builder.Configuration["OpenApi:info:contact:url"]),
    //        Email = builder.Configuration["OpenApi:info:contact:email"]
    //    },
    //    License = new OpenApiLicense
    //    {
    //        Name = builder.Configuration["OpenApi:info:License:name"],
    //        Url = new Uri(builder.Configuration["OpenApi:info:License:url"])
    //    }
    //});
});


//Dependencias propias de Servicio
builder.Services.RegisterDependencies();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();



builder.Services.AddHealthChecks();
TokenValidationHandler.SetupJWTServices(builder);

//Parametros Entorno
try
{
    var _appname = "BGHDBANCAPYMEVariables";
    var values = new Dictionary<string, string>();
    foreach (DictionaryEntry e in Environment.GetEnvironmentVariables())
    {
        if (e.Key != null)
        {
            var _key = e.Key.ToString().Trim();

            if (_key.StartsWith(_appname + "_"))
            {
                var _name = _key.Replace(_appname + "_", "");
                try { values.Add(_name, e.Value.ToString()); } catch (Exception) { }
            }
        }
    }
    builder.Host.ConfigureAppConfiguration((ctx, _builder) =>
    {
        _builder.AddInMemoryCollection(values);
    });

}
catch (Exception) { }

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
        c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"v2");
    });
}


app.ConfigureMetricServer();
app.ConfigureExceptionHandler();


app.UseRouting();

app.UseCors("myAllowSpecificOrigins"); //la variable añadida para el front

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health/readiness", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
});

app.MapHealthChecks("/health/liveness", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    Predicate = _ => false
});



app.MapControllers();




app.Run();
