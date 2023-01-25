using bg.hd.banca.pyme.domain.entities.ArchivosImpuesto;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IArchivosImpuestoRestRepository
    {
        Task<ArchivosImpuestoResponse> ValidarArchivoImpuesto(ArchivosImpuestoRequest request);
    }
}
