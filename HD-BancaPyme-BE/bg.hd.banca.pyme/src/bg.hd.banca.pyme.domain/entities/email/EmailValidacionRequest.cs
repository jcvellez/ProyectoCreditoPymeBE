using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.email
{
    public class EmailValidacionRequest
    {
        [JsonIgnore]
        public string? codigoAppBg { get; set; }
        /// <summary>
        /// identificacion
        /// </summary>
        /// <example>0703691824</example>
        public string? identificacion { get; set; }
        /// <summary>
        /// correo
        /// </summary>
        /// <example>correoprueba@gmail.com</example>
        public string? correo { get; set; }
    }
}
