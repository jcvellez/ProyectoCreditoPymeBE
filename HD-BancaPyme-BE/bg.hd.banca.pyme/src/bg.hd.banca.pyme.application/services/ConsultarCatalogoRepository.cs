using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.catalogo;
using System.Linq;

namespace bg.hd.banca.pyme.application.services
{
    public class ConsultarCatalogoRepository : IConsultarCatalogoRepository
    {
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;
        public ConsultarCatalogoRepository(IConsultarCatalogoRestRepository consultarCatalogoRestRepository)
        {
            _consultarCatalogoRestRepository = consultarCatalogoRestRepository;
        }

        public async Task<ConsultarCatalogoResponse> ConsultarCatalogo(ConsultarCatalogoRequest request)
        {
            ConsultarCatalogoResponse _response = new();
        
            _response = await _consultarCatalogoRestRepository.ConsultarCatalogo(request);

            if (request.opcion == 1 && request.idCatalogo == "47" && request.idCatalogoPadre == "0")
            {

                List<CatalogoDetalle> lista = _response.listaCatalogoDetalle.catalogoDetalle.Where(x => x.strValor4 == "S").Select(x => { x.strValor = x.strValor5; return x;} ).ToList();

                _response.listaCatalogoDetalle.catalogoDetalle = lista;
            }
            

            return _response;
        }

        public async Task<ConsultarCatalogoResponse> ConsultarCatalogoFiltrado(ConsultarCatalogoRequestMicroServ request, string clienteToken) 
        {
            return await _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(request);
        }
    }
}
