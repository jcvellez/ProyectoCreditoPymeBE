using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaClientesProveedoresRequest
    {
        public string? Identificacion { get; set; }
        public int? TipoClienteProveedor { get; set; } = 0;
    }
}
