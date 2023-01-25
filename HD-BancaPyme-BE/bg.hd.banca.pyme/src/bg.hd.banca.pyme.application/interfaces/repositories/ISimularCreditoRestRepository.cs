using bg.hd.banca.pyme.domain.entities.SimularCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface ISimularCreditoRestRepository
    {
        //SimularCreditoResponse SimularCredito(SimularCreditoRequest request);
        Task<SimularCreditoResponse> SimularCredito_CuotaMensual(SimularCreditoRequest request);
        Task<SimularCreditoResponse> SimularCredito_AlVencimiento(SimularCreditoRequest request, double tasaNominal);
        Task<ConsultaTasaInteresResponse> ConsultaTasaInteres(ConsultaTasaInteresRequest request);
    }
}
