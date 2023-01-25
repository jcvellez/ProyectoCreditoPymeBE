using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.indicador
{
    public class GarantiaActualizarResponse
    {
        public int? CodigoRetorno { get; set; }
        public string? Mensaje { get; set; }
        public string? BandIngresoManual { get; set; }
    }
}
