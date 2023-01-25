using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.SimularCredito
{
    public class SimularCreditoRequestMicroServ
    {
        public int? tipoIdentificacion { get; set; }
        public string? identificacion { get; set; }
        public int? monto { get; set; }
        public string? idProducto { get; set; }
        public int? diaPago { get; set; }
        public int? plazo { get; set; }
        public string? tipoTabla { get; set; }
        public string? usuario { get; set; }
        public int? enviaCorreo { get; set; }
        public int? idSolicitud { get; set; }                
    }
}
