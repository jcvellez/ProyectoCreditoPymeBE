using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva
{
    public class ConsultarMesesIvaResquest
    {
        public int? consultaAnyos { get; set; } = 1;        
        public int? idProceso { get; set; } = 0;
    }
}
