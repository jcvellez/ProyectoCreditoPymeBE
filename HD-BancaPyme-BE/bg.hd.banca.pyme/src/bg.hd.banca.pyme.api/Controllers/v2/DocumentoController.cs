using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.documento;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bg.hd.banca.pyme.api.Controllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]
    public class DocumentoController : BaseApiAuthController
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IConfiguration _configuration;
        private string clienteToken = String.Empty;

        public DocumentoController(IDocumentoRepository _documentoRepository, IConfiguration _configuration)
        {
            this._documentoRepository = _documentoRepository;
            this._configuration = _configuration;
        }

       
        [HttpPost]
        [Route("hdbancapyme/v2/documento/digitalizar-documento")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<DigitalizarDocumentosResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<DigitalizarDocumentosResponse>>> DigitalizarDocumentos([FromBody] DigitalizarDocumentosRequest request)
        {
            
            DigitalizarDocumentosResponse response = await _documentoRepository.DigitalizarDocumentos(request);

            return Ok(new MsDtoResponse<DigitalizarDocumentosResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        [HttpPost]
        [Route("hdbancapyme/v2/documento/generar-contratos-documentos")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<GenerarDocumentosCreditoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 401)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<GenerarDocumentosCreditoResponse>>> GenerarDocumentosContratoscredito([FromBody] GenerarDocumentosCreditoRequest _request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.Claims.FirstOrDefault(c => c.Type == "clientid")?.Value == _request.Identificacion)
            {
                GenerarDocumentosCreditoResponse _response = await _documentoRepository.GenerarDocumentosContratoscredito(_request);
                return Ok(new MsDtoResponse<GenerarDocumentosCreditoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");

        }
    }
    
}
