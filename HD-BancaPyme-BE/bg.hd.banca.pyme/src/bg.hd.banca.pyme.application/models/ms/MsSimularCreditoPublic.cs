using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.ms
{
    public class MsSimularCreditoPublic
    {
        public string? mensaje { get; set; }
        public double? cuota { get; set; }
        public double? tasaInteres { get; set; }
        public double? totalPagar { get; set; }
        public int? plazo { get; set; }
    }
}
