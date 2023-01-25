using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.indicador
{
    public class GarantiaActualizarRequest
    {
        public int? idProceso { get; set; }
        public string? identificacion { get; set; }
        public string? tipoIdentificacion { get; set; }
    }
}
