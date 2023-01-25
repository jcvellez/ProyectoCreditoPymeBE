using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.expediente.detalle;
using bg.hd.banca.pyme.domain.entities.persona;
//using bg.hd.banca.pyme.domain.entities;


namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IGestionExpedienteRestRepository
    {
        Task<CrearExpedienteResponse> IngresoExpediente(IngresoExpedienteRequest request);
        Task<CrearSolicitudResponse> CrearSolicitud(CrearSolicitudRequest request);
        Task<ModificarSolicitudResponse> ModificarSolicitud(ModificarSolicitudRequest request);
        Task<Solicitud> ConsultarSolicitud(SolicitudRequest request);
        Task<ValidarPoliticasResponse> ValidarPoliticas(ValidarPoliticasRequest request);
        Task<ConsultarConfiguracionResponse> ConsultarConfiguracion(ConsultarConfiguracionRequest request);
        Task<Transaccion> ModificarExpediente(ModificarExpedienteRequest request);
        Task<ActualizarExpedienteResponse> ActualizarExpediente(ActualizarExpedienteRequest request);
        Task<ParametrosGeneralesResponse> ParametrosGenerales(ParametrosGeneralesRequest request);
        Task<Producto> ObtenerProducto(string productoId);
        Task<Producto> ObtenerIdProducto(string productoId);
        Task<VerificaSolicitudesResponseMicro> RetomarSolicitud(RetomarSolicitudRequest request);
        Task<ConsultarSectorPymesResponse> SectoresVetados(string identificacion);
        Task<ConsultaGuidPersonaResponse> ConsultaGuidPersona(string identificacion);
        Task<ValidaFirmaElectronicaResponse> ValidaCreaSolicitudesProducto(ValidaFirmaElectronicaRequest request);
        Task<ConsultarExpProductoActResponse> consultaExpedientesId(ConsultarExpProductoActRequest request);
        Task<EncolarExpProcesoNeoBatchResponse> EncolarProcesoNeoBatch(EncolarExpProcesoNeoBatchRequest request);
        Task<ActualizaExpedientesDetalleResponse> ActualizaExpedientesDetalles(ActualizaExpedientesDetalleRequest request);


    }
}