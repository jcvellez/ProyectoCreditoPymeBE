using bg.hd.banca.pyme.domain.entities.indicador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IIndicadorGenerarRestRepository
    {
        Task<IndicadorGenerarResponse> IndicadorGenerar(IndicadorGenerarRequest request);
        Task<GarantiaActualizarResponse> GarantiaActualizar(IndicadorGenerarRequest request);
    }
}
