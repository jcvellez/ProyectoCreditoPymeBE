using bg.hd.banca.pyme.domain.entities.SimularCreditoPublic;
using bg.hd.banca.pyme.domain.entities.SimularCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface ISimularCreditoPublicRestRepository
    {        
        Task<SimularCreditoPublicResponse> SimularCreditoPublic_CuotaMensual(SimularCreditoPublicRequest request);
        Task<SimularCreditoPublicResponse> SimularCreditoPublic_AlVencimiento(SimularCreditoPublicRequest request, double tasaNominal);
        Task<ConsultaTasaInteresResponse> ConsultaTasaInteres(ConsultaTasaInteresRequest request);
    }
}
