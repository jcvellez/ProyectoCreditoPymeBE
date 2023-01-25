using bg.hd.banca.pyme.domain.entities.SimularCreditoPublic;
using bg.hd.banca.pyme.domain.entities.SimularCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface ISimularCreditoPublicRepository
    {        
        Task<SimularCreditoPublicResponse> SimularCreditoPublic(SimularCreditoPublicRequest request);

        
    }
}
