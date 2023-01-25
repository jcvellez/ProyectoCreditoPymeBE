using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaInstalacionesSegurosResponse
    {
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = "";        
        public string propio { get; set; } = "";        
        public string companiaAseguradora { get; set; }
        public int NumeroEmpleados { get; set; } = 0;
    }
}
