using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.documento
{
    public class GenerarDocumentosCreditoRequest
    {
        /// <summary>
        /// Identificacion del cliente
        /// </summary>
        /// <example>0917398190</example>
        [Required]
        public string? Identificacion { get; set; } = "";
        /// <summary>
        /// IdExpediente del expediente del cliente
        /// </summary>
        /// <example>9819298</example>
        [Required]
        public int? IdExpediente { get; set; } = 0;
        /// <summary>
        /// producto app banco
        /// </summary>
        /// <example>cuotaMensual</example>
        [Required]
        public string? producto { get; set; } = "";
        /// <summary>
        /// marca app banco
        /// </summary>
        /// <example>C</example>
        [JsonIgnoreAttribute]
        public string? marca { get; set; } = "C";
        public string UsuarioGestor { get; set; } = "";
        /// <summary>
        /// solicitud del cliente
        /// </summary>
        /// <example>228966</example>
        [Required]
        public string? IdSolicitud { get; set; } = "";
    }
}