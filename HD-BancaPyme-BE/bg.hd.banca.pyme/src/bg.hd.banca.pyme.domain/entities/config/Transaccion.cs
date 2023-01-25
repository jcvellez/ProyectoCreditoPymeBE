using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.config
{
    public class Transaccion
    {
        public int codigoResponse { get; set; }
        public string MessageResponse { get; set; } = "";
        public string DescriptionResponse { get; set; } = "";
    }

}
