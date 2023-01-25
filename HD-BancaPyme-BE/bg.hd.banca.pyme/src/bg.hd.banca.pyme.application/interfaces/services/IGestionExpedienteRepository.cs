using bg.hd.banca.pyme.domain.entities.expediente;
//using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.expediente;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IGestionExpedienteRepository
    {
        Task<CrearExpedienteResponse> CrearExpediente(CrearExpedienteRequest request);
        Task<CrearSolicitudResponse> CrearSolicitud(CrearSolicitudRequest request);
        Task<ModificarSolicitudResponse> ModificarSolicitud(ModificarSolicitudRequest request);
        Task<Solicitud> ConsultarSolicitud(string identificacion, string idSolicitud);
        Task<ActualizarExpedienteResponse> ActualizarExpediente(ActualizarExpedienteRequest request);
        Task<ParametrosGeneralesResponse> ParametrosGenerales(ParametrosGeneralesRequest request);
        Task<RetomarSolicitudResponse> RetomarSolicitud(RetomarSolicitudRequest request);        
        Task<CerrarExpedienteResponse> CerrarExpediente(CerrarExpedienteRequest request);
        Task<ActualizarSolicitudPantallaResponse> ActualizarSolicitudPantalla(ActualizarSolicitudPantallaRequest request);
        Task<ConsultaGuidPersonaResponse> ConsultaGuidPersona(string identificacion);
        Task<ValidaFirmaElectronicaResponse> ValidaCreaSolicitudesProducto(ValidaFirmaElectronicaRequest request);
        Task<ConsultarExpProductoActResponse> consultaExpedientesId(ConsultarExpProductoActRequest request);
        Task<EncolarExpProcesoNeoBatchResponse> EncolarProcesoNeoBatch(EncolarExpProcesoNeoBatchRequest request);
        Task<ActualizaSolicitudResponse> ActualizaSolicitud(ActualizarSolicitudRequest request);

    }
}