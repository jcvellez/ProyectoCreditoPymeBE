using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class GuardarDatosNegocioResponse
    {
        public int CodigoRetorno { get; set; }
        public string? Mensaje { get; set; }
        public string? idPersona { get; set; }
    }
}
