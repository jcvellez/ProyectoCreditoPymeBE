using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.domain.entities.ArchivosImpuesto;
using bg.hd.banca.pyme.domain.entities.indicador;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;
using bg.hd.banca.pyme.domain.entities.PreCalificador;
using bg.hd.banca.pyme.domain.entities.ClientesRatingNeo;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.informacionCliente;

namespace bg.hd.banca.pyme.api.Controllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]
    public class PreCalificador : BaseApiAuthController
    {
        private readonly IAnalisisRatingRepository _clientesratingneoRepository;
        private readonly IArchivosImpuestoRepository _archivosImpuestoRepository;
        private readonly IArchivoIvaRepository _archivoIvaRepository;
        private readonly IIndicadorGenerarRepository _indicadorgenerarRepository;
        private readonly IConsultaMesesIvaRepository _consultarMesesIvaRepository;
        private readonly IPreCalificadorRepository _preCalificadorRepository;
        private readonly IConfiguration _configuration;
        private string clienteToken = String.Empty;
        public PreCalificador(IAnalisisRatingRepository _clientesratingneoRepository, IArchivosImpuestoRepository _archivosImpuestoRepository, IConfiguration Configuration, IIndicadorGenerarRepository _indicadorgenerarRepository, IArchivoIvaRepository _archivoIvaRepository, IConsultaMesesIvaRepository _consultarMesesIvaRepository, IPreCalificadorRepository _preCalificadorRepository)
        {
            this._archivosImpuestoRepository = _archivosImpuestoRepository;
            this._configuration = Configuration;
            this._indicadorgenerarRepository = _indicadorgenerarRepository;
            this._archivoIvaRepository = _archivoIvaRepository;
            this._consultarMesesIvaRepository = _consultarMesesIvaRepository;
            this._preCalificadorRepository = _preCalificadorRepository;
            this._clientesratingneoRepository = _clientesratingneoRepository;
        }


        [HttpPost]
        [Route("hdbancapyme/v2/ClientesRatingNeo")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ClientesRatingNeoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ClientesRatingNeoResponse>>> CrearAnalisis([FromBody] ClientesRatingNeoRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                ClientesRatingNeoResponse _response = await _clientesratingneoRepository.CrearAnalisis(request);
                return new MsDtoResponse<ClientesRatingNeoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower());
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/archivoImpuestoRenta")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ArchivosImpuestoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ArchivosImpuestoResponse>>> ValidarImpuestoRenta([FromBody][Required] ArchivosImpuestoRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                ArchivosImpuestoResponse _response = await _archivosImpuestoRepository.ValidarArchivoImpuesto(request);
                return Ok(new MsDtoResponse<ArchivosImpuestoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        //IndicadorGenerar
        [HttpPost]
        [Route("hdbancapyme/v2/indicador-generar")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ArchivosImpuestoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GarantiaActualizarResponse>>> IndicadorGenerar([FromBody][Required] IndicadorGenerarRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                GarantiaActualizarResponse _response = await _indicadorgenerarRepository.IndicadorGenerar(request);
                return Ok(new MsDtoResponse<GarantiaActualizarResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }
        [HttpPost]
        [Route("hdbancapyme/v2/IvaPDFProcesar")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ArchivoImpuestoIvaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ArchivoImpuestoIvaResponse>>> ValidarArchivoIva([FromBody][Required] ArchivoImpuestoIvaRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                ArchivoImpuestoIvaResponse _response = await _archivoIvaRepository.ValidarArchivoIva(request);
                return Ok(new MsDtoResponse<ArchivoImpuestoIvaResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpGet]
        [Route("hdbancapyme/v2/balance-meses")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultarMesesIvaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultarMesesIvaResponse>>> ConsultarMesesIva(int tipoConsulta, int idProceso)
        {
            Request.Headers.TryGetValue(String.Format(_configuration["Security:tokenName"]), out var token);
            clienteToken = token;
            ConsultarMesesIvaResponse _response = await _consultarMesesIvaRepository.ConsultarMesesIva(tipoConsulta, idProceso);
            return Ok(new MsDtoResponse<ConsultarMesesIvaResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }

        [HttpPost]
        [Route("hdbancapyme/v2/PreCalificar")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<PreCalificadorResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<PreCalificadorResponse>>> ValidarArchivoIva([FromBody][Required] PreCalificadorRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.identificacion)
            {
                PreCalificadorResponse _response = await _preCalificadorRepository.PreCalificar(request);
                return Ok(new MsDtoResponse<PreCalificadorResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }

        [HttpPost]
        [Route("hdbancapyme/v2/precalificador/cuentas-contables")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<CuentasContablesResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<CuentasContablesResponse>>> IngresoCuentasContables([FromBody][Required] CuentasContablesRequest request)
        {
            //Request.Headers.TryGetValue(string.Format(_configuration["Security:tokenName"]), out var token);
            CuentasContablesResponse response = await _preCalificadorRepository.IngresoCuentasContables(request);
            return Ok(new MsDtoResponse<CuentasContablesResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpPost]
        [Route("hdbancapyme/v2/declaracion-semestral-manual")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<IngresoDeclaracionSemestralResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<IngresoDeclaracionSemestralResponse>>> IngresoDeclaracionSemestral([FromBody][Required] IngresoDeclaracionSemestralRequest request)
        {
            IngresoDeclaracionSemestralResponse _response = await _archivoIvaRepository.IngresoDeclaracionSemestral(request);
            return Ok(new MsDtoResponse<IngresoDeclaracionSemestralResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));

        }
    }
}