using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.pyme.domain.entities.BancaControl;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.persona;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IInformacionClienteRepository
    {
        Task<InformacionClienteResponse> InformacionCliente(string identificacion);
        Task<InformacionClienteDniResponse> ConsultarDatosRC(string identificacion, string producto);        
        Task<ConsultaDatosNegocioResponse> ConsultarDatosNegocio(string identificacion);
        Task<GuardarDatosNegocioResponse> GuardarDatosNegocio(GuardarDatosNegocioRequest request);
        Task<IngresarDetalleVentasResponse> IngresarDetalleVentas(IngresarDetalleVentasRequest request);
        Task<ConsultarDetalleVentasResponse> ConsultarDetalleVentas(string identificacion);
        Task<GrabaClientesProveedoresResponse> GrabaProveedorCliente(GrabaClientesProveedoresRequest request);
        Task<ConsultaClientesProveedoresResponse> ConsultarClientesProveedores(string identificacion, int tipoClienteProveedor);
        Task<GrabaReferenciaBancariaResponse> GrabaReferenciasBancarias(GrabaReferenciaBancariaRequest request);
        Task<ConsultaReferenciaBancariaResponse> ConsultaReferenciasBancarias(string identificacion);
        Task<ConsultarCuentaPorIdResponse> ConsultarCuentaPorId(string identificacion);
        Task<ConsultarDetalleCertificadosResponse> ConsultarCertificacionesAmbientales(string identificacion);
        Task<GrabaCertificacionAmbientalResponse> GrabaCertificacionesAmbientales(GrabaCertificacionAmbientalRequest request);
        Task<ConsultaInstalacionesSegurosResponse> ConsultaInstalacionSeguros(string identificacion);
        Task<GrabaInstalacionesSegurosResponse> GrabaInstalacionSeguros(GrabaInstalacionesSegurosRequest request);
        Task<BancaControlResponse> ConsultarEstadoTarjetaVirtual(BancaControlRequest request, bool controlarException);
        Task<GestionarDatosNormativosReponse> GestionarDatosNormativos(GestionarDatosNormativosRequest request);
        Task<ConsultaResidenciaFiscalResponse> ConsultaResidenciaFiscal(ConsultaResidenciaFiscalRequest request);
        Task<GestionValidarPoliticasResponse> GestionValidarPoliticas(GestionValidarPoliticasRequest request);
        Task<List<ConsultaContratoPorCanalResponse>> ConsultaContratoPorCanal(string identificacion, string canal);
        Task<RegistroContratosResponse> RegistroContratos(RegistroContratosRequest request);
    }
}
