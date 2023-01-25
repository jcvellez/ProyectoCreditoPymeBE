using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.token.acceso;
using Microsoft.Extensions.Configuration;

namespace bg.hd.banca.pyme.application.services
{
    public class TokenAccesoRepository : ITokenAccesoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenAccesoRestRepository _tokenAccesoRestRepository;

        public TokenAccesoRepository(ITokenAccesoRestRepository tokenAccesoRestRepository,
           IConfiguration _configuration)
        {
            this._configuration = _configuration;
            this._tokenAccesoRestRepository = tokenAccesoRestRepository;
        }
        public async Task<ValidaTokenAccesoCanalApliResponse> ValidarTokenAccesoCanalAplicacion(ValidaTokenAccesoCanalApliRequest request)
        {
            return await _tokenAccesoRestRepository.ValidarTokenAccesoCanalAplicacion(request);
        }
    }
}
