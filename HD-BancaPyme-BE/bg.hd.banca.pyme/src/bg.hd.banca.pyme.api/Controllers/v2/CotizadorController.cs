using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.SimularCredito;
using Microsoft.AspNetCore.Mvc;

namespace bg.hd.banca.pyme.api.Controllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]
    public class CotizadorController : BaseApiAuthController
    {
        private readonly ISimularCreditoRepository _simularcreditoRepository;
        private readonly IConfiguration _configuration;
        private string clienteToken = String.Empty;
        public CotizadorController(ISimularCreditoRepository _simularcreditoRepository, IConfiguration Configuration)
        {
            this._simularcreditoRepository = _simularcreditoRepository;
            this._configuration = Configuration;

        }

        [HttpPost]
        [Route("hdbancapyme/v2/SimularCredito")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<SimularCreditoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<SimularCreditoResponse>>> SimularCredito_CuotaMensual([FromBody] SimularCreditoRequest request)
        {
            var currentUser = HttpContext.User;

            if (currentUser?.FindFirst(c => c.Type == "clientid")?.Value == request.cedula)
            {
                SimularCreditoResponse _response = await _simularcreditoRepository.SimularCredito(request);
                return new MsDtoResponse<SimularCreditoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower());
            }
            else
                throw new CustomUnauthorizedExeption("Unauthorized", "invalid_clientidtoken");
        }
    }
}
