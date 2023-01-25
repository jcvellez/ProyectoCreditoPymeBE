using bg.hd.banca.pyme.domain.entities.ArchivosImpuesto;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IArchivosImpuestoRepository
    {
        Task<ArchivosImpuestoResponse> ValidarArchivoImpuesto(ArchivosImpuestoRequest request);
    }
}
