using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.SimularCreditoPublic
{
    public class SimularCreditoPublicRequestMicroServ
    {
        //[JsonIgnore]
        //public int? tipoIdentificacion { get; set; }
        //[JsonIgnore]
        //public string? identificacion { get; set; }
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
