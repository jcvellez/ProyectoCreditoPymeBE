using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;
using bg.hd.banca.pyme.domain.entities.FichaPreCalificador;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.PreCalificador;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IPreCalificadorRestRepository
    {
        Task<PreCalificadorResponse> ConsultaHostRiesgos(PreCalificadorRequest request);
        Task<PreCalificadorResponse> GenerarAnalisisCualitativo(PreCalificadorRequest request);
        Task<PreCalificadorResponse> GenerarCalificacionSBS(PreCalificadorRequest request);
        Task<GenerarFichaPreCalificadorResponse> GenerarFichaPreCalificador(PreCalificadorRequest request);
        Task<PreCalificadorResponse> InformeFinalSBS(PreCalificadorRequest request);
        Task<PreCalificadorResponse> GuardarFichaPrecalificador(PreCalificadorRequest request);
        Task<ProcDeclaracionIVAResponse> ProcesoDeclaracionIva(PreCalificadorRequest request);
        Task<CuentasContablesResponse> IngresoCuentasContables(CuentasContablesRequest request);        
    }
}
