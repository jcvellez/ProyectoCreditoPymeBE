using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;
using bg.hd.banca.pyme.application.models.exeptions;

namespace bg.hd.banca.pyme.application.services
{
    public class ConsultaMesesIvaRepository: IConsultaMesesIvaRepository
    {
        private readonly IConsultaMesesIvaRestRepository _consultaMesesIvaRestRepository;

        public ConsultaMesesIvaRepository(IConsultaMesesIvaRestRepository _consultaMesesIvaRestRepository)
        {
            this._consultaMesesIvaRestRepository = _consultaMesesIvaRestRepository;
        }

        public async Task<ConsultarMesesIvaResponse> ConsultarMesesIva(int tipoConsulta, int idProceso)
        {
   

            return await _consultaMesesIvaRestRepository.ConsultarMesesIva(tipoConsulta, idProceso);
        }

    }
}
