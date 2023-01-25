using bg.hd.banca.pyme.domain.entities.token.acceso;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface ITokenAccesoRestRepository
    {
        Task<ValidaTokenAccesoCanalApliResponse> ValidarTokenAccesoCanalAplicacion(ValidaTokenAccesoCanalApliRequest request);
    }
}
