using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.email;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.domain.entities.otp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.biometria;
using System.Net;
using bg.hd.banca.pyme.infrastructure.utils;

namespace bg.hd.banca.pyme.api.Controllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]
    public class Informacion : BaseApiAuthController
    {
        private readonly IInformacionClienteRepository _informacionClienteRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailValidacionRepository _emailValidacionRespository;
        private readonly IBiometriaRepository _biometriaRepository;
        private string clienteToken = String.Empty;
        public Informacion(IInformacionClienteRepository _informacionClienteRepository, IConfiguration Configuration, 
            IEmailValidacionRepository _emailValidacionRespository, IBiometriaRepository _biometriaRepository)
        {
            this._informacionClienteRepository = _informacionClienteRepository;
            this._configuration = Configuration;
            this._emailValidacionRespository = _emailValidacionRespository;
            this._biometriaRepository = _biometriaRepository;
        }

        [HttpGet]
        [Route("hdbancapyme/v2/informacion-cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<InformacionClienteResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<InformacionClienteResponse>>> InformacionCliente([FromHeader][Required] string identificacion)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == identificacion)
            {

                InformacionClienteResponse _response = await _informacionClienteRepository.InformacionCliente(identificacion);
                return Ok(new MsDtoResponse<InformacionClienteResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/email/validacion")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<EmailValidacionResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<EmailValidacionResponse>>> ValidarEmail([FromBody][Required] EmailValidacionRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {


                EmailValidacionResponse _response = await _emailValidacionRespository.ValidarEmail(request);
                return Ok(new MsDtoResponse<EmailValidacionResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }


        [HttpGet]
        [Route("hdbancapyme/v2/informacion/datos-negocio")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultaDatosNegocioResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultaDatosNegocioResponse>>> ConsultaDatosNegocio([FromHeader][Required] string identificacion)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == identificacion)
            {

                ConsultaDatosNegocioResponse response = await _informacionClienteRepository.ConsultarDatosNegocio(identificacion);

                return Ok(new MsDtoResponse<ConsultaDatosNegocioResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/informacion/datos-negocio")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GuardarDatosNegocioResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GuardarDatosNegocioResponse>>> GuardarDatosNegocio([FromBody][Required] GuardarDatosNegocioRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                GuardarDatosNegocioResponse response = await _informacionClienteRepository.GuardarDatosNegocio(request);
                return Ok(new MsDtoResponse<GuardarDatosNegocioResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/informacion/detalles-ventas")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<IngresarDetalleVentasResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<IngresarDetalleVentasResponse>>> IngresarDetalleVentas([FromBody][Required] IngresarDetalleVentasRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.Identificacion)
            {

                IngresarDetalleVentasResponse response = await _informacionClienteRepository.IngresarDetalleVentas(request);

                return Ok(new MsDtoResponse<IngresarDetalleVentasResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpGet]
        [Route("hdbancapyme/v2/informacion/detalles-ventas")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultarDetalleVentasResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultarDetalleVentasResponse>>> ConsultarDetalleVentas([FromHeader][Required] string identificacion)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == identificacion)
            {
                ConsultarDetalleVentasResponse response = await _informacionClienteRepository.ConsultarDetalleVentas(identificacion);

                return Ok(new MsDtoResponse<ConsultarDetalleVentasResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }


        [HttpPost]
        [Route("hdbancapyme/v2/informacion/proveedor-cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GrabaClientesProveedoresResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GrabaClientesProveedoresResponse>>> GrabaProveedorCliente([FromBody][Required] GrabaClientesProveedoresRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.Identificacion)
            {
                GrabaClientesProveedoresResponse response = await _informacionClienteRepository.GrabaProveedorCliente(request);
                return Ok(new MsDtoResponse<GrabaClientesProveedoresResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpGet]
        [Route("hdbancapyme/v2/informacion/proveedor-cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultaClientesProveedoresResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultaClientesProveedoresResponse>>> ConsultarClientesProveedores([FromHeader][Required] string identificacion, int tipoClienteProveedor)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.Claims.FirstOrDefault(c => c.Type == "clientid")?.Value == identificacion)
            {
                ConsultaClientesProveedoresResponse response = await _informacionClienteRepository.ConsultarClientesProveedores(identificacion, tipoClienteProveedor);
                return Ok(new MsDtoResponse<ConsultaClientesProveedoresResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/informacion/referencias-bancarias")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GrabaReferenciaBancariaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GrabaReferenciaBancariaResponse>>> GrabaReferenciasBancarias([FromBody][Required] GrabaReferenciaBancariaRequest request)
        {
            //Request.Headers.TryGetValue(string.Format(_configuration["Security:tokenName"]), out var token);
            GrabaReferenciaBancariaResponse response = await _informacionClienteRepository.GrabaReferenciasBancarias(request);
            return Ok(new MsDtoResponse<GrabaReferenciaBancariaResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        [HttpGet]
        [Route("hdbancapyme/v2/informacion/referencias-bancarias")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultaReferenciaBancariaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultaReferenciaBancariaResponse>>> ConsultaReferenciasBancarias([FromHeader][Required] string identificacion)
        {
            Request.Headers.TryGetValue(string.Format(_configuration["Security:tokenName"]), out var token);

            ConsultaReferenciaBancariaResponse response = await _informacionClienteRepository.ConsultaReferenciasBancarias(identificacion);

            return Ok(new MsDtoResponse<ConsultaReferenciaBancariaResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        [HttpGet]
        [Route("hdbancapyme/v2/informacion/cuentas-cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultarCuentaPorIdResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultarCuentaPorIdResponse>>> ConsultarCuentaPorId([FromHeader][Required] string identificacion)
        {
            Request.Headers.TryGetValue(string.Format(_configuration["Security:tokenName"]), out var token);

            ConsultarCuentaPorIdResponse response = await _informacionClienteRepository.ConsultarCuentaPorId(identificacion);

            return Ok(new MsDtoResponse<ConsultarCuentaPorIdResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        [HttpGet]
        [Route("hdbancapyme/v2/informacion/certificaciones-ambientales")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultarDetalleCertificadosResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultarDetalleCertificadosResponse>>> ConsultarDetalleCertificados([FromHeader][Required] string identificacion)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.Claims.FirstOrDefault(c => c.Type == "clientid")?.Value == identificacion)
            {
                ConsultarDetalleCertificadosResponse response = await _informacionClienteRepository.ConsultarCertificacionesAmbientales(identificacion);
                return Ok(new MsDtoResponse<ConsultarDetalleCertificadosResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/informacion/certificaciones-ambientales")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GrabaCertificacionAmbientalResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GrabaCertificacionAmbientalResponse>>> GrabaCertificacionesAmbientales([FromBody][Required] GrabaCertificacionAmbientalRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.Identificacion)
            {
                GrabaCertificacionAmbientalResponse response = await _informacionClienteRepository.GrabaCertificacionesAmbientales(request);
                return Ok(new MsDtoResponse<GrabaCertificacionAmbientalResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/informacion/gestionar-informacion-normativa")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GestionarDatosNormativosReponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GestionarDatosNormativosReponse>>> GestionarDatosNormativos([FromBody] GestionarDatosNormativosRequest request)
        {
            GestionarDatosNormativosReponse _response = await _informacionClienteRepository.GestionarDatosNormativos(request);
            return Ok(new MsDtoResponse<GestionarDatosNormativosReponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
        /// <summary>    
        [HttpGet]
        [Route("hdbancapyme/v2/informacion/residencias-fiscales")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultaResidenciaFiscalResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultaResidenciaFiscalResponse>>> ConsultarResidenciasFiscales([FromHeader][Required] string identificacion)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.Claims.FirstOrDefault(c => c.Type == "clientid")?.Value == identificacion)
            {
                ConsultaResidenciaFiscalRequest request = new ConsultaResidenciaFiscalRequest();
                request.opcion = "2";
                request.identidad = identificacion;
                ConsultaResidenciaFiscalResponse response = await _informacionClienteRepository.ConsultaResidenciaFiscal(request);
                return Ok(new MsDtoResponse<ConsultaResidenciaFiscalResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/informacion/gestion-validar-politicas")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GestionValidarPoliticasResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GestionValidarPoliticasResponse>>> GestionValidarPoliticas([FromBody] GestionValidarPoliticasRequest request)
        {
            GestionValidarPoliticasResponse _response = await _informacionClienteRepository.GestionValidarPoliticas(request);
            return Ok(new MsDtoResponse<GestionValidarPoliticasResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
    }
}
