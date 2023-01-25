using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva
{
    public class ConsultarMesesIvaResponse
    {
        public int? CodigoRetorno { get; set; }
        public string? Mensaje { get; set; }
        public string? mes_1 { get; set; }
        public string? mes_2 { get; set; }
        public string? mes_3 { get; set; }
        public bool semestreManual { get; set; } = false;
    }
}
