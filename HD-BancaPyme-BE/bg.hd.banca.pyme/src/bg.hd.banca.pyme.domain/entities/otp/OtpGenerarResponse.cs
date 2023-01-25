using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.otp
{
    public class OtpGenerarResponse
    {
        [JsonIgnore] 
        //public string OtpEncriptado { get; set; }
        public string CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        ///public string Otp { get; set; }

    }
}
