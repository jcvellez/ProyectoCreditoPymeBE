using bg.hd.banca.pyme.domain.entities.firmaElectronica;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IFirmaElectronicaRestRepository
    {
        Task<GenerarCertificadoFirmaElectronicaResponse> GenerarCertificadoFirmaElectronica(GenerarCertificadoFirmaElectronicaRequest request);
        Task<RegistrarAuditoriaResponse> RegistrarAuditoriaFirmaElectronica(RegistrarAuditoriaRequest request);
        Task<ConfirmacionCertificadoResponse> ConfirmarCertificadoFirmaElectronica(ConfirmacionCertificadoRequest request);
    }
}
