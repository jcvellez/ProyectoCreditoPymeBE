using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;


namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IArchivoIvaRepository
    {
        Task<ArchivoImpuestoIvaResponse> ValidarArchivoIva(ArchivoImpuestoIvaRequest request);
        Task<IngresoDeclaracionSemestralResponse> IngresoDeclaracionSemestral(IngresoDeclaracionSemestralRequest request);
    }
}
