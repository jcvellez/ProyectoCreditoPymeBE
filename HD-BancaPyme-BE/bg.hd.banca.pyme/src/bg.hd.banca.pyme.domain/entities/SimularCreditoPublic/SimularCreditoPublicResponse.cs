using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.SimularCreditoPublic
{
    public class SimularCreditoPublicResponse
    {
        public int? codigoRetorno { get; set; }
        public string? mensaje { get; set; }
        public double? cuota { get; set; }
        public double? tasaInteres { get; set; }
        public double? totalPagar { get; set; }
        public int? plazo { get; set; }
    }
}
