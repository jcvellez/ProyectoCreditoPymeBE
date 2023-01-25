using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IConsultaMesesIvaRestRepository
    {
        Task<ConsultarMesesIvaResponse> ConsultarMesesIva(int tipoConsulta, int idProceso);
    }
}
