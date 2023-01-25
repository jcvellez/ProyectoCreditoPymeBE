using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IArchivoIvaRestRepository
    {
        Task<ArchivoImpuestoIvaResponse> ValidarArchivoIva(ArchivoImpuestoIvaRequest request);
        Task<IngresoDeclaracionSemestralResponse> IngresoDeclaracionSemestral(IngresoDeclaracionSemestralRequest request);
    }
}
