using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.expediente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace bg.hd.banca.pyme.api.Controllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]
    public class Expediente : BaseApiAuthController
    {

        private readonly IGestionExpedienteRepository _gestionExpedienteRepository;
        private readonly IConfiguration _configuration;
        private string clienteToken = String.Empty;

        public Expediente(IGestionExpedienteRepository _gestionExpedienteRepository,
                            IConfiguration _configuration)
        {
            this._gestionExpedienteRepository = _gestionExpedienteRepository;
            this._configuration = _configuration;
        }

        [HttpPost]
        [Route("hdbancapyme/v2/expediente/crear-solicitud")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<CrearSolicitudResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<CrearSolicitudResponse>>> CrearSolicitud([FromBody][Required] CrearSolicitudRequest request)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.Identificacion)
            {
                CrearSolicitudResponse _response = await _gestionExpedienteRepository.CrearSolicitud(request);

                return Ok(new MsDtoResponse<CrearSolicitudResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/expediente/crear-expediente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<CrearExpedienteResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<CrearExpedienteResponse>>> CrearExpediente([FromBody][Required] CrearExpedienteRequest request)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.Identificacion)
            {

                CrearExpedienteResponse _response = await _gestionExpedienteRepository.CrearExpediente(request);

                return Ok(new MsDtoResponse<CrearExpedienteResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpGet]
        [Route("hdbancapyme/v2/expediente/consultar-solicitud")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<Solicitud>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<Solicitud>>> ConsultarSolicitud([FromHeader][Required] string identificacion, [FromHeader][Required] string idSolicitud)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == identificacion)
            {

                Solicitud _response = await _gestionExpedienteRepository.ConsultarSolicitud(identificacion, idSolicitud);

                return Ok(new MsDtoResponse<Solicitud>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/expediente/modificar-expediente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ActualizarExpedienteResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ActualizarExpedienteResponse>>> ActualizarExpediente([FromBody][Required] ActualizarExpedienteRequest request)
        {
            var currentUser = HttpContext.User;
            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                ActualizarExpedienteResponse _response = await _gestionExpedienteRepository.ActualizarExpediente(request);

                return Ok(new MsDtoResponse<ActualizarExpedienteResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }




        [HttpPost]
        [Route("hdbancapyme/v2/expediente/cerrar-expediente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<CerrarExpedienteResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<CerrarExpedienteResponse>>> CerrarExpediente([FromBody][Required] CerrarExpedienteRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.Identificacion)
            {
                CerrarExpedienteResponse _response = await _gestionExpedienteRepository.CerrarExpediente(request);

                return Ok(new MsDtoResponse<CerrarExpedienteResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/expediente/pantalla-solicitud")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ActualizarSolicitudPantallaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ActualizarSolicitudPantallaResponse>>> ActualizarSolicitudPantalla([FromBody][Required] ActualizarSolicitudPantallaRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.Identificacion)
            {

                ActualizarSolicitudPantallaResponse _response = await _gestionExpedienteRepository.ActualizarSolicitudPantalla(request);

                return Ok(new MsDtoResponse<ActualizarSolicitudPantallaResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/expediente/modifica-solicitud")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ActualizaSolicitudResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ActualizaSolicitudResponse>>> ActualizaSolicitud([FromBody][Required] ActualizarSolicitudRequest request)
        {

            ActualizaSolicitudResponse _response = await _gestionExpedienteRepository.ActualizaSolicitud(request);

                return Ok(new MsDtoResponse<ActualizaSolicitudResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
          
        }
    }
}