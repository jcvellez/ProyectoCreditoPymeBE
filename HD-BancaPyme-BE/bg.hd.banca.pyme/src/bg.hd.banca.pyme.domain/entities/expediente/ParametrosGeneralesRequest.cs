using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ParametrosGeneralesRequest
    {
        public int opcion { get; set; }
        public string? idProducto { get; set; }
        public string? idCodigo { get; set; }
    }
}
