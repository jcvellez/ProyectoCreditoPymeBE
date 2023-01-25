using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.indicador;
using bg.hd.banca.pyme.application.models.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.services
{
    public class IndicadorGenerarRepository : IIndicadorGenerarRepository
    {
        private readonly IIndicadorGenerarRestRepository _indicadorgenerarRestRepository;

        public IndicadorGenerarRepository(IIndicadorGenerarRestRepository _indicadorgenerarRestRepository)
        {
            this._indicadorgenerarRestRepository = _indicadorgenerarRestRepository;
        }
        public async Task<GarantiaActualizarResponse> IndicadorGenerar(IndicadorGenerarRequest request)
        {
            if (request.definitivo != "S" && request.definitivo != "N")
            {
                throw new IndicadorGenerarException("Valor de campo definitivo no permitido", "Valor de campo definitivo no permitido", 2);
            }
            GarantiaActualizarResponse garantiaActualizar = new();
            IndicadorGenerarResponse indicadoGenerar = new();

            indicadoGenerar = await _indicadorgenerarRestRepository.IndicadorGenerar(request);
            garantiaActualizar = await _indicadorgenerarRestRepository.GarantiaActualizar(request);

            garantiaActualizar.BandIngresoManual = indicadoGenerar.BandIngresoManual;
            garantiaActualizar.Mensaje = indicadoGenerar.Mensaje;

            return garantiaActualizar;
        }
    }
}
