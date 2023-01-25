using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaInstalacionesSegurosRequest
    {
        public string? Identificacion { get; set; }
        public int? Propio { get; set; }
        public int? CompaniaAseguradora { get; set; }
        public int? NumeroEmpleados { get; set; } = 0;        
    }
}
