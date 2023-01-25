using bg.hd.banca.pyme.domain.entities.persona;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class ConsultaActividadEconomicaResponse
    {
        public string? codigoCIUU { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }
        public string? ciudad { get; set; }
        public string? referencia { get; set; }
    }
}
