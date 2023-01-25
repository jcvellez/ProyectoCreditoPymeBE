using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.SimularCredito
{
    public class ConsultaTasaInteresRequest
    {
        public int? idCanal { get; set; }
        public int? idProducto { get; set; }
        public int? PeriodicidadDias { get; set; }
    }
}
