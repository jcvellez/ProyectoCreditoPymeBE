using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.pyme.domain.entities.BancaControl;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities.crmCasos;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.persona;


namespace bg.hd.banca.pyme.application.interfaces.repositories
{

    public interface IInformacionClienteRestRepository
    {
        Task<InformacionClienteDniResponse> InformacionCliente(string identificacion, string codDactilar);
        Task<IdentificaUsuarioDataResponse> InformacionClienteData(string identificacion);
        Task<ConsultaPersonaResponse> InformacionClientePersona(string identificacion);
        Task<IdentificaNombres> ObtenerNombres(string nombres);
        Task<ConsultaOficialResponseMicroServ> ConsultarDatosOficial(ConsultarOficialRequest parameter);
        Task<Transaccion> GrabarDatosNormativos(ActualizaInformacionNormativaRequest request);
        Task<ConsultaDatosSRIResponse> ConsultaDatosSRI(string identificacion);
        Task<GrabarDatosPersonaResponse> GrabarDatosPersona(GrabarDatosPersonaRequest request);        
        Task<ActualizaVentasClienteResponse> ActualizarVentasCliente(ActualizaVentasClienteMicroServ request);
        Task<ConsultaActividadEconomicaResponse> ConsultaActividadEconomica(string identificacion);
        Task<IngresarDetalleVentasResponse> IngresarDetalleVentas(IngresarDetalleVentasRequest request);
        Task<ConsultarDetalleVentasResponse> ConsultarDetalleVentas(string identificacion);
        Task<GrabaClientesProveedoresResponse> GrabaProveedorCliente(GrabaClientesProveedoresRequest request);
        Task<ConsultaClientesProveedoresResponse> ConsultarClientesProveedores(string identificacion,int tipoClienteProveedor);
        Task<GrabaReferenciaBancariaResponse> GrabaReferenciasBancarias(GrabaReferenciaBancariaRequest request);
        Task<ConsultaReferenciaBancariaResponse> ConsultaReferenciasBancarias(string identificacion);
        Task<ConsultarCuentaPorIdResponse> ConsultarCuentaPorId(string identificacion);
        Task<ConsultarDetalleCertificadosResponse> ConsultarCertificacionesAmbientales(string identificacion);
        Task<GrabaCertificacionAmbientalResponse> GrabaCertificacionesAmbientales(GrabaCertificacionAmbientalRequest request);
        Task<ConsultaInstalacionesSegurosResponse> ConsultaInstalacionSeguros(string identificacion);
        Task<GrabaInstalacionesSegurosResponse> GrabaInstalacionSeguros(GrabaInstalacionesSegurosRequest request);
        Task<BancaControlResponse> ConsultarEstadoTarjetaVirtual(BancaControlRequest request, bool controlarException);
        Task<ConsultaDatosRUCResponse> ConsultaDatosRUC(string identificacion);
        Task<ActualizaPatrimonioResponse> ActualizarDataCrm(ActualizaPatrimonioRequest request);
        Task<GestionResidenciaFiscalResponse> GestionarResidenciaFiscal(GestionResidenciaFiscalRequest request);
        Task<ConsultaResidenciaFiscalResponse> ConsultaResidenciaFiscal(ConsultaResidenciaFiscalRequest request);
        Task<DatosRCResponse> ConsultaRCDatos(string identificacion);
        Task<ActualizaRFCrmResponse> ActualizaResidenciaFiscalCrm(ActualizaRFCrmRequest request);
        Task<List<ConsultaContratoPorCanalResponse>> ConsultaContratoPorCanal(string identificacion, string canal);
        Task<RegistroContratosResponse> RegistroContratos(RegistroContratosRequest request);        
    }
}