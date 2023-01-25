using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.domain.entities.catalogo;
using Microsoft.AspNetCore.Mvc;

namespace bg.hd.banca.pyme.api.Controllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]    
    public class CatalogoController : BaseApiAuthController
    {
        private readonly IConsultarCatalogoRepository _consultarcatalogoRepository;
        private readonly IConfiguration _configuration;
        private string clienteToken = String.Empty;

        public CatalogoController(IConsultarCatalogoRepository _consultarcatalogoRepository, IConfiguration Configuration)
        {
            this._consultarcatalogoRepository = _consultarcatalogoRepository;
            this._configuration = Configuration;
        }
        [HttpPost]
        [Route("hdbancapyme/v2/catalogo/consultar-catalogo")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultarCatalogoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultarCatalogoResponse>>> ConsultarCatalogo([FromBody] ConsultarCatalogoRequest request)
        {
            ConsultarCatalogoResponse _response = await _consultarcatalogoRepository.ConsultarCatalogo(request);
            return new MsDtoResponse<ConsultarCatalogoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower());
        }       
    }
}
