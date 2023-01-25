using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.otp
{
        public class OtpValidarRequest
        {
        /// <example>1102036355</example>
        public string? identificacion { get; set; }
        /// <example></example>
        public string otp { get; set; }

        }

    
}
