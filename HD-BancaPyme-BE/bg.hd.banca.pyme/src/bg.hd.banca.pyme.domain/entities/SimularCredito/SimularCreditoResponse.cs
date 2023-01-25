using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.SimularCredito
{
    public class SimularCreditoResponse
    {        
        public int? codigoRetorno { get; set; }
        public string? mensaje { get; set; }
        public double? cuota { get; set; }
        public double? tasaInteres { get; set; }
        public double? tasaInteresSolicitada { get; set; }
        public double? totalPagar { get; set; } 
        public double? monto { get; set; }
        public string? tipoProducto { get; set; }
        public string? tipoCuota { get; set; }
        public int? diaPago { get; set; }
        public int? plazo { get; set; }
    }
}
