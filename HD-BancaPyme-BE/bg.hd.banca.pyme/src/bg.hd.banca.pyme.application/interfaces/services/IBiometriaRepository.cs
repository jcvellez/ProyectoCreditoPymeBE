using bg.hd.banca.pyme.domain.entities.biometria;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IBiometriaRepository
    {
        Task<RegistroBiometriaResponse> RegistroBiometria(RegistroBiometriaRequest request);

        Task<ValidaBiometriaResponse> ValidaBiometria(ValidaBiometriaRequest request);

        Task<ImagenTokenizadaResponse> GestionarBiometria(ImagenTokenizadaRequest request);
    }
}
