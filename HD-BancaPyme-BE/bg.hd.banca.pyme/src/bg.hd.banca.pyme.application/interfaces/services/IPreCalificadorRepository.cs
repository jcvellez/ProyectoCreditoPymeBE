using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.PreCalificador;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IPreCalificadorRepository
    {
        Task<PreCalificadorResponse> PreCalificar(PreCalificadorRequest request);
        Task<CuentasContablesResponse> IngresoCuentasContables(CuentasContablesRequest request);
    }
}