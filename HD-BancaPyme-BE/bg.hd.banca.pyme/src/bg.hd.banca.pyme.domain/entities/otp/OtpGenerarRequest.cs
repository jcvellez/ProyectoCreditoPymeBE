using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.otp
{
    public class OtpGenerarRequest
    {
        /// <example>1102036355</example>
        public string? Identificacion { get; set; }

    }
}
