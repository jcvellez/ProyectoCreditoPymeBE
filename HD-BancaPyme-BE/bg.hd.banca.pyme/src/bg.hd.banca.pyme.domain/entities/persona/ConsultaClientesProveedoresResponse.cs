using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaClientesProveedoresResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        public List<DetallesClienteProveedor>? Personas { get; set; }
        public class DetallesClienteProveedor
        {
            public int idPersona { get; set; }

            public string? nombre { get; set; }

            public string? porcentaje { get; set; }
            public string? plazo { get; set; }
        }
    }
}
