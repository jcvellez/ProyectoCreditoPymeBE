using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.FichaPreCalificador
{
    public class GenerarFichaPreCalificadorResponse
    {
        public int? CodigoRetorno { get; set; }
        public string? Mensaje { get; set; }
        public string? Dictamen { get; set; }
        public string montoAprobado { get; set; }
    }
}
