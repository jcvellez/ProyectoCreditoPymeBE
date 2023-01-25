using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.otp
{
    public class OtpGenerarRequestMicroServ
    {
        public string? LlaveOTP { get; set; }
        public string? IvOTP { get; set; }
        public string? Aplicacion { get; set; }
        public string? Servicio { get; set; }
        public string? Canal { get; set; }
        public string? OpidOTP { get; set; }
        public string? Terminal { get; set; }
        public string? Identificacion { get; set; }
        public string? TipoIdentificacion { get; set; }
        public string? Notificacion { get; set; }
        public string? SmsOpid { get; set; }
        public string? SmsOrigen { get; set; }
        public string? EmailOrigen { get; set; }
        public string? EmailAsunto { get; set; }
        public string? Template { get; set; }
    }
}
