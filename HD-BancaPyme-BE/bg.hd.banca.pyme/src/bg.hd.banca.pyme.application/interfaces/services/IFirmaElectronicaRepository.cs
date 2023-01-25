using bg.hd.banca.pyme.domain.entities.firmaElectronica;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IFirmaElectronicaRepository
    {
        Task<GenerarCertificadoFirmaElectronicaResponse> GenerarCertificadoFirmaElectronica(GenerarCertificadoFirmaElectronicaRequest request);
        Task<RegistrarAuditoriaResponse> RegistrarAuditoriaFirmaElectronica(RegistrarAuditoriaRequest request);
        Task<ConfirmacionCertificadoResponse> ConfirmarCertificadoFirmaElectronica(ConfirmacionCertificadoRequest request);

    }
}
