using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.SimularCreditoPublic
{
    public class SimularCreditoPublicRequest
    {
        /// <example>15000</example>
        public int? monto { get; set; }
        /// <example>capitalTrabajo</example>
        public string? destinoFinanciero { get; set; }
        /// <example>alVencimiento</example>
        public string? tipoProducto { get; set; }
        /// <example>180</example>
        public int? plazo { get; set; }
        /// <example>fija</example>
        public string? tipoCuota { get; set; }
        /// <example>null</example>
        public int? diaPago { get; set; }
    }
}
