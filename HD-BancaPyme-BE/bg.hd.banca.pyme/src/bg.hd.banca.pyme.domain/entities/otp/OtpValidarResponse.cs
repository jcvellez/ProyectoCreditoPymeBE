using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace bg.hd.banca.pyme.domain.entities.otp
{
    public class OtpValidarResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        [JsonProperty("client-token")]
        public string jwtCliente { get; set; }
    }
}
