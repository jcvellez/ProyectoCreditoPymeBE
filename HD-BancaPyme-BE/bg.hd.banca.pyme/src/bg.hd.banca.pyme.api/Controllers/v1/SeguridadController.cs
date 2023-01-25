using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.domain.entities.recaptcha;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.domain.entities.otp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using bg.hd.banca.pyme.domain.entities.biometria;
using System.Net;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.token.acceso;
using bg.hd.banca.pyme.domain.entities.informacionCliente;

namespace bg.hd.banca.pyme.api.Controllers.v1
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class Seguridad : BaseApiController
    {
        private readonly IRecaptchaRepository _recaptchaRepository;
        private readonly IIdentificaUsuarioRepository _identificaUsuarioRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly IBiometriaRepository _biometriaRepository;
        private readonly IGestionExpedienteRepository _gestionExpedienteRepository;
        private readonly ITokenAccesoRepository _tokenAccesoRepository;
        private readonly IInformacionClienteRepository _informacionClienteRepository;        
        public Seguridad(IGestionExpedienteRepository _gestionExpedienteRepository, IRecaptchaRepository _recaptchaRepository, IIdentificaUsuarioRepository _identificaUsuarioRepository, IOtpRepository _otpRepository, IBiometriaRepository _biometriaRepository, ITokenAccesoRepository _tokenAccesoRepository, IInformacionClienteRepository _informacionClienteRepository)
        {
            this._recaptchaRepository = _recaptchaRepository;
            this._identificaUsuarioRepository = _identificaUsuarioRepository;
            this._otpRepository = _otpRepository;
            this._biometriaRepository = _biometriaRepository;
            this._gestionExpedienteRepository = _gestionExpedienteRepository;
            this._tokenAccesoRepository = _tokenAccesoRepository;
            this._informacionClienteRepository = _informacionClienteRepository;            
        }

        /// <summary>
        /// Valida Recaptcha
        /// </summary>
        [HttpPost]
        [Route("hdbancapyme/v1/seguridad/validacion-recaptcha")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<RecaptchaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<RecaptchaResponse>>> validarRecaptcha([FromBody][Required] RecaptchaRequest request)
        {
            RecaptchaResponse response = await _recaptchaRepository.validarRecaptcha(request);
            return Ok(new MsDtoResponse<RecaptchaResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        /// <summary>
        /// Identifica Cliente
        /// </summary>
        [HttpGet]
        [Route("hdbancapyme/v1/seguridad/identificar-usuario")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<IdentificaUsuarioResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<IdentificaUsuarioResponse>>> IdentificarUSuario([FromHeader][Required] string identificacion, string? codDactilar, bool vieneSolicitud)
        {
            IdentificaUsuarioResponse response = await _identificaUsuarioRepository.IdentificarUsuario(identificacion, codDactilar, "C", true, vieneSolicitud);
            return Ok(new MsDtoResponse<IdentificaUsuarioResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
        /// <summary>
        /// Genera Otp Cliente
        /// </summary>
        [HttpPost]
        [Route("hdbancapyme/v1/seguridad/generar-otp")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<OtpGenerarResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<OtpGenerarResponse>>> GenerarOtp([FromBody][Required] OtpGenerarRequest request)
        {

            OtpGenerarResponse response = await _otpRepository.GenerarOtp(request);

            return Ok(new MsDtoResponse<OtpGenerarResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
        /// <summary>
        /// Valida Otp Cliente
        /// </summary>
        [HttpPost]
        [Route("hdbancapyme/v1/seguridad/validar-otp")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<RecaptchaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<RecaptchaResponse>>> ValidarOtp([FromBody][Required] OtpValidarRequest request)
        {
            OtpValidarResponse response = await _otpRepository.ValidarOtp(request);
            return Ok(new MsDtoResponse<OtpValidarResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
        /// <summary>
        /// Valida Biometria y Genera Token
        /// </summary>
        [HttpPost]
        [Route("hdbancapyme/v1/seguridad/validar-biometria")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ImagenTokenizadaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ImagenTokenizadaResponse>>> GestionarBiometria([FromBody][Required] ImagenTokenizadaRequest request)
        {
            var currentUser = HttpContext.User;

            var headers = HttpContext.Request.Headers;
            if (headers.ContainsKey("X-Forwarded-For"))
            {
                request.IpCliente = IPAddress.Parse(headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]).ToString();
            }

            ImagenTokenizadaResponse response = await _biometriaRepository.GestionarBiometria(request);
            return Ok(new MsDtoResponse<ImagenTokenizadaResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));


        }
        /// <summary>
        /// Consulta Datos en Proceso
        /// </summary>
        [HttpPost]
        [Route("hdbancapyme/v1/seguridad/en-proceso")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<RetomarSolicitudResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<RetomarSolicitudResponse>>> RetomarSolicitud([FromBody][Required] RetomarSolicitudRequest request)
        {
            RetomarSolicitudResponse response = await _gestionExpedienteRepository.RetomarSolicitud(request);
            return Ok(new MsDtoResponse<RetomarSolicitudResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));

        }

        /// <summary>
        /// Metodo GET (Valida Token de Acceso proveniente de Otro Canal o Aplicativo)
        /// </summary>
        [HttpGet]
        [Route("hdbancapyme/v1/seguridad/validar-token-acceso")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ValidaTokenAccesoCanalApliResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ValidaTokenAccesoCanalApliResponse>>> ValidarTokenAccesoCanalAplicacion([FromHeader][Required] string CanalId, [FromHeader][Required] string AplicacionId, [FromHeader][Required] string TkValue)
        {
            ValidaTokenAccesoCanalApliRequest _request = new ValidaTokenAccesoCanalApliRequest();
            _request.CanalId = CanalId;
            _request.AplicacionId = AplicacionId;
            _request.TkValue = TkValue;
            ValidaTokenAccesoCanalApliResponse _response = await _tokenAccesoRepository.ValidarTokenAccesoCanalAplicacion(_request);
            return Ok(new MsDtoResponse<ValidaTokenAccesoCanalApliResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        /// <summary>
        /// Consulta Contrato por Identificacion
        /// </summary>
        [HttpGet]
        [Route("hdbancapyme/v1/seguridad/canal")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<List<ConsultaContratoPorCanalResponse>>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<List<ConsultaContratoPorCanalResponse>>>> ConsultaContratoPorCanal([FromHeader][Required] string identificacion, string canal)
        {
            List<ConsultaContratoPorCanalResponse> response = await _informacionClienteRepository.ConsultaContratoPorCanal(identificacion, canal);
            return Ok(new MsDtoResponse<List<ConsultaContratoPorCanalResponse>>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        /// <summary>
        /// Registro de Contactos
        /// </summary>
        [HttpPost]
        [Route("hdbancapyme/v1/seguridad/concentimientos")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<RegistroContratosResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<RegistroContratosResponse>>> RegistroContratos([FromBody][Required] RegistroContratosRequest request)
        {

            RegistroContratosResponse response = await _informacionClienteRepository.RegistroContratos(request);

            return Ok(new MsDtoResponse<RegistroContratosResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }
    }
}
