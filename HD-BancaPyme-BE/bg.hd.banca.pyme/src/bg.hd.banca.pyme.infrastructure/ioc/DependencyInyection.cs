using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.infrastructure.data.repositories;
using bg.hd.banca.pyme.infrastructure.seguridad;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using bg.hd.banca.pyme.application.services;

namespace bg.hd.banca.pyme.infrastructure.ioc
{
    public static class DependencyInyection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            Log.Logger = new LoggerConfiguration()
                  .ReadFrom
                  .Configuration(configuration).CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));


            _ = int.TryParse(configuration["Jaeger:Telemetry:Port"], out int portNumber);
            services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                .AddSource(configuration["Serilog:Properties:Application"])
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: configuration["Serilog:Properties:Application"]))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Enrich = (activity, eventName, rawObject) =>
                    {
                        string? traceid = string.Empty;
                        if (rawObject is HttpRequest httpRequest)
                        {
                            traceid = httpRequest.HttpContext?.TraceIdentifier;
                            activity.SetTag("Serilog-Traceid", traceid);
                        }
                    };
                })
                .AddJaegerExporter(opts =>
                {
                    opts.AgentHost = configuration["Jaeger:Telemetry:Host"];
                    opts.AgentPort = portNumber;
                });
            });


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IOtpRestRepository, OtpRestRepository>();
            services.AddScoped<ISimularCreditoRestRepository, SimularCreditoRestRepository>();
            services.AddScoped<IRecaptchaRestRepository, RecaptchaRestRepository>();
            services.AddScoped<IIdentificaUsuarioRestRepository, IdentificaUsuarioRestRepository>();
            services.AddScoped<IInformacionClienteRestRepository, InformacionClienteRestRepository>();
            services.AddScoped<ISimularCreditoPublicRestRepository, SimularCreditoPublicRestRepository>();
            services.AddScoped<IAnalisisRatingRestRepository, AnalisisRatingRestRepository>();
            services.AddScoped<IArchivosImpuestoRestRepository, ArchivosImpuestoRestRepository>();
            services.AddScoped<IEmailValidacionRestRepository, EmailValidacionRestRepository>();
            services.AddScoped<IIndicadorGenerarRestRepository, IndicadorGenerarRestRepository>();
            services.AddScoped<IArchivoIvaRestRepository, ArchivoIvaRestRepository>();
            services.AddScoped<IConsultaMesesIvaRestRepository, ConsultarMesesIvaRestRepository>();
            services.AddScoped<IPreCalificadorRestRepository, PreCalificadorRestRepository>();
            services.AddScoped<IGestionExpedienteRestRepository, GestionExpedienteRestRepository>();
            services.AddScoped<IConsultarCatalogoRestRepository, ConsultarCatalogoRestRepository>();
            services.AddScoped<IDocumentoRestRepository, DocumentoRestRepository>();
            services.AddScoped<IBiometriaRestRepository, BiometriaRestRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IFirmaElectronicaRestRepository, FirmaElectronicaRestRepository>();
            services.AddScoped<ITokenAccesoRestRepository, TokenAccesoRestRepository>();

            return services;
        }
    }
}
