using bg.hd.banca.pyme.domain.entities.biometria;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IBiometriaRestRepository
    {
        Task<RegistroBiometriaResponse> RegistroBiometria(RegistroBiometriaRequest request);

        Task<ValidaBiometriaResponse> ValidaBiometria(ValidaBiometriaRequest request);

        Task<ImagenTokenizadaResponse> GestionarBiometria(ImagenTokenizadaRequest request);
        Task<BiometriaTrazabilidadResponse> ConsultaInformacionTrazabilidad(BiometriaTrazabilidadRequest request);

    }
}
