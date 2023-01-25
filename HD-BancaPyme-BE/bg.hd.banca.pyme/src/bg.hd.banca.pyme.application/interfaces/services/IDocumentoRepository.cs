using bg.hd.banca.pyme.domain.entities.documento;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IDocumentoRepository
    {
        Task<DigitalizarDocumentosResponse> DigitalizarDocumentos(DigitalizarDocumentosRequest request);
        Task<GenerarDocumentosCreditoResponse> GenerarDocumentosContratoscredito(GenerarDocumentosCreditoRequest request);

    }
}
