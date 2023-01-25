using bg.hd.banca.pyme.domain.entities.documento;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IDocumentoRestRepository
    {
        Task<DigitalizarDocumentosResponse> DigitalizarDocumentos(DigitalizarDocumentosRequest request);
        Task<GenerarDocumentosCreditoResponse> GenerarDocumentosContratoscredito(GenerarDocumentosCreditoRequest request);

    }
}
