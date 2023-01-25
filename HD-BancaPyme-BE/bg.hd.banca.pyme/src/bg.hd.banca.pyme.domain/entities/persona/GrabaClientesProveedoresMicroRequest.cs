using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaClientesProveedoresMicroRequest
    {

        public string? Opcion { get; set; }
        public string? Identificacion { get; set; }
        public int? TipoClienteProveedor { get; set; } = 0;
        public List<DetallesPersonas>? Personas { get; set; }
    }
}
