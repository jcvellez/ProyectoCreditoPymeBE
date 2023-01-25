using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;


namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IConsultaMesesIvaRepository
    {
        Task<ConsultarMesesIvaResponse> ConsultarMesesIva(int tipoConsulta, int idProceso);
    }
}
