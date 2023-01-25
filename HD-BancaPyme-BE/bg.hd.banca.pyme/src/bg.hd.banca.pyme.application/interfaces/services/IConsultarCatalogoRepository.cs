using bg.hd.banca.pyme.domain.entities.catalogo;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IConsultarCatalogoRepository
    { 
         Task<ConsultarCatalogoResponse> ConsultarCatalogo(ConsultarCatalogoRequest request);
         Task<ConsultarCatalogoResponse> ConsultarCatalogoFiltrado(ConsultarCatalogoRequestMicroServ request, string clienteToken);
    }
}
