using bg.hd.banca.pyme.domain.entities.catalogo;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IConsultarCatalogoRestRepository
    {
        Task<ConsultarCatalogoResponse>ConsultarCatalogo(ConsultarCatalogoRequest request);
        Task<ConsultarCatalogoResponse> ConsultarCatalogoFiltrado(ConsultarCatalogoRequestMicroServ request);
    }
}
