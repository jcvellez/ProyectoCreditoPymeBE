using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaInstalacionesSegurosMicroRequest
    {
        public string? Identificacion { get; set; }
        public string? Opcion { get; set; }
        public int? Propio { get; set; }
        public int? CompaniaSeguro { get; set; }
        public int? NumeroEmpleados { get; set; }
        public int? idParroquia { get; set; }
    }
}
