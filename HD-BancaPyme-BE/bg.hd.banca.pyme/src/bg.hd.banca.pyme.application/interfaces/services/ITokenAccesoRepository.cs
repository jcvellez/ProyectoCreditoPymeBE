using bg.hd.banca.pyme.domain.entities.token.acceso;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface ITokenAccesoRepository
    {
        Task<ValidaTokenAccesoCanalApliResponse> ValidarTokenAccesoCanalAplicacion(ValidaTokenAccesoCanalApliRequest request);
    }
}
