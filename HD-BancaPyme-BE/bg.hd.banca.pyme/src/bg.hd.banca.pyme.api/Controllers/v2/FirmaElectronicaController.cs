using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.firmaElectronica;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace bg.hd.banca.pyme.api.Controllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]
    public class FirmaElectronica : BaseApiAuthController
    {
        private readonly IFirmaElectronicaRepository _firmaElectronicaRepository;
        private readonly IConfiguration _configuration;

        public FirmaElectronica(IFirmaElectronicaRepository _firmaElectronicaRepository,
                            IConfiguration _configuration)
        {
            this._firmaElectronicaRepository = _firmaElectronicaRepository;
            this._configuration = _configuration;
        }

        [HttpPost]
        [Route("hdbancapyme/v2/firma-electronica/certificado/generar")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GenerarCertificadoFirmaElectronicaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GenerarCertificadoFirmaElectronicaResponse>>> GenerarCertificadoFirmaElectronica([FromBody][Required] GenerarCertificadoFirmaElectronicaRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.Claims.FirstOrDefault(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                var headers = HttpContext.Request.Headers;
                if (headers.ContainsKey("X-Forwarded-For"))
                {
                    request.ip = IPAddress.Parse(headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]).ToString();
                }
                GenerarCertificadoFirmaElectronicaResponse _response = await _firmaElectronicaRepository.GenerarCertificadoFirmaElectronica(request);
                return Ok(new MsDtoResponse<GenerarCertificadoFirmaElectronicaResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/firma-electronica/certificado/auditoria")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<RegistrarAuditoriaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<RegistrarAuditoriaResponse>>> RegistrarAuditoriaFirmaElectronica([FromBody][Required] RegistrarAuditoriaRequest request)
        {
            var currentUser = HttpContext.User;

            var headers = HttpContext.Request.Headers;
            if (headers.ContainsKey("X-Forwarded-For"))
            {
                request.ip = IPAddress.Parse(headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]).ToString();
            }

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                RegistrarAuditoriaResponse _response = await _firmaElectronicaRepository.RegistrarAuditoriaFirmaElectronica(request);
                return Ok(new MsDtoResponse<RegistrarAuditoriaResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        /// <summary>
        /// Firma Documentos Electronicos
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Request de producto neoWeb
        ///     {
        ///        "producto": "cuotaMensual",
        ///        "idExpediente": "9819298",
        ///        "identificacion": "0917398190",
        ///        "clave": "wfvfvfvj0j9817989"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("hdbancapyme/v2/firma-electronica/certificado/confirmar-certificado")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConfirmacionCertificadoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConfirmacionCertificadoResponse>>> ConfirmarCertificadoFirmaElectronica([FromBody][Required] ConfirmacionCertificadoRequest request)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.Claims.FirstOrDefault(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                var headers = HttpContext.Request.Headers;
                if (headers.ContainsKey("X-Forwarded-For"))
                {
                    request.ip = IPAddress.Parse(headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]).ToString();
                }
                ConfirmacionCertificadoResponse _response = await _firmaElectronicaRepository.ConfirmarCertificadoFirmaElectronica(request);
                return Ok(new MsDtoResponse<ConfirmacionCertificadoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }
    }
}
