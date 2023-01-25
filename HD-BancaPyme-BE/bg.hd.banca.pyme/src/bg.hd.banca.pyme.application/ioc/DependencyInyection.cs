using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.services;
using Microsoft.Extensions.DependencyInjection;

namespace bg.hd.banca.pyme.application.ioc
{
    public static class DependencyInyection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<ISimularCreditoRepository, SimularCreditoRepository>();
            services.AddScoped<IRecaptchaRepository, RecaptchaRepository>();
            services.AddScoped<IIdentificaUsuarioRepository, IdentificaUsuarioRepository>();
            services.AddScoped<IInformacionClienteRepository, InformacionClienteRepository>();
            services.AddScoped<ISimularCreditoPublicRepository, SimularCreditoPublicRepository>();
            services.AddScoped<IAnalisisRatingRepository, AnalisisRatingRepository>();
            services.AddScoped<IArchivosImpuestoRepository, ArchivosImpuestoRepository>();
            services.AddScoped<IEmailValidacionRepository, EmailValidacionRepository>();
            services.AddScoped<IIndicadorGenerarRepository, IndicadorGenerarRepository>();
            services.AddScoped<IArchivoIvaRepository, ArchivoIvaRepository>();
            services.AddScoped<IConsultaMesesIvaRepository, ConsultaMesesIvaRepository>();
            services.AddScoped<IPreCalificadorRepository, PreCalificadorRepository>();
            services.AddScoped<IGestionExpedienteRepository, GestionExpedienteRepository>();
            services.AddScoped<IConsultarCatalogoRepository, ConsultarCatalogoRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IBiometriaRepository, BiometriaRepository>();
            services.AddScoped<IAuthenticationServiceRepository, AuthenticationServiceRepository>();
            services.AddScoped<IFirmaElectronicaRepository, FirmaElectronicaRepository>();
            services.AddScoped<ITokenAccesoRepository, TokenAccesoRepository>();

            return services;
        }
    }
}
