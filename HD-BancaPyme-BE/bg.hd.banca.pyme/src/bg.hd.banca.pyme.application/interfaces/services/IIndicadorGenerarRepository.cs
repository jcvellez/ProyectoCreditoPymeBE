using bg.hd.banca.pyme.domain.entities.indicador;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IIndicadorGenerarRepository
    {
        Task<GarantiaActualizarResponse> IndicadorGenerar(IndicadorGenerarRequest request);
    }
}
