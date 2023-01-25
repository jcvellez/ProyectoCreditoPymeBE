using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.domain.entities.SimularCreditoPublic;
using Microsoft.AspNetCore.Mvc;

namespace bg.hd.banca.pyme.api.Controllers.v1
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class CotizadorPublicController : BaseApiController
    {
        private readonly ISimularCreditoPublicRepository _simularcreditopublicRepository;

        public CotizadorPublicController(ISimularCreditoPublicRepository _simularcreditopublicRepository)
        {
            this._simularcreditopublicRepository = _simularcreditopublicRepository;
        }

        /// <summary>
        /// Simula Credito Pyme
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Request al Vencimiento
        ///     {
        ///        
        ///         "destinoFinanciero": "capitalTrabajo"
        ///         "diaPago": null
        ///         "monto": 15000
        ///         "plazo": 180
        ///         "tipoCuota": "fija"
        ///         "tipoProducto": "alVencimiento"
        ///     }
        ///
        ///     POST /Request cuotaMensual
        ///     {
        ///         "diaPago": 1
        ///         "monto": 15000
        ///         "plazo": 36
        ///         "tipoCuota": "fija"
        ///         "tipoProducto": "cuotaMensual"
        ///     }
        /// </remarks>

        [HttpPost]
        [Route("hdbancapyme/v1/SimularCreditoPublic")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<SimularCreditoPublicResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<SimularCreditoPublicResponse>>> SimularCredito([FromBody] SimularCreditoPublicRequest request)
        {
            //object _response = _simularcreditopublicRepository.SimularCreditoPublic(request);
            SimularCreditoPublicResponse _response = await _simularcreditopublicRepository.SimularCreditoPublic(request);
            return new MsDtoResponse<SimularCreditoPublicResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower());
        }
    }
}
