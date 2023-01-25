using bg.hd.banca.pyme.domain.entities.ClientesRatingNeo;
using bg.hd.banca.pyme.domain.entities.ExcepcionAnalisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IAnalisisRatingRestRepository
    {
        Task<ClientesRatingNeoResponse> CrearAnalisis(ClientesRatingNeoRequest request);
        Task<AnyosBalanceResponse> ConsultarAnyosBalance(AnyosWebRequest request);
        Task<ExcepcionAnalisisResponse> GenerarExcepcionAnalisis(ExcepcionAnalisisRequest request);
    }
}
