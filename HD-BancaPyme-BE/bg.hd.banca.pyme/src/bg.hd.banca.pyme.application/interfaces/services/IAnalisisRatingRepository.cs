using bg.hd.banca.pyme.domain.entities.ClientesRatingNeo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IAnalisisRatingRepository
    {
        Task<ClientesRatingNeoResponse> CrearAnalisis(ClientesRatingNeoRequest request);
        

    }
}
