using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.firmaElectronica
{
    public class GenerarCertificadoFirmaElectronicaRequest
    {
        [JsonIgnoreAttribute]
        public string opcion { get; set; } = "1";
        /// <summary>
        /// producto app banco
        /// </summary>
        /// <example>cuotaMensual</example>
        [Required]
        public string? producto { get; set; } = "";
        /// <summary>
        /// IdExpediente del expediente del cliente
        /// </summary>
        /// <example>9819298</example>
        [Required]
        public string? IdExpediente { get; set; } = "0";
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0917398190</example>
        [Required]
        [StringLength(10, ErrorMessage = "La identificación debe tener 10 caracteres", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La identificación debe ser númerica")]
        public string identificacion { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Oscar</example>           
        [JsonIgnoreAttribute]
        public string nombre { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Paguay</example>           
        [JsonIgnoreAttribute]
        public string apellidoPaterno { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>           
        [JsonIgnoreAttribute]
        public string apellidoMaterno { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>           
        [JsonIgnoreAttribute]
        public string direccion { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>           
        [JsonIgnoreAttribute]
        public string telefono { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>           
        [JsonIgnoreAttribute]
        public string provincia { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>           
        [JsonIgnoreAttribute]
        public string ciudad { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>           
        [JsonIgnoreAttribute]
        public string email { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>
//        [JsonIgnoreAttribute]
      //  public string producto { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>      
        [JsonIgnoreAttribute]
        public string ip { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>Torres</example>           
        [JsonPropertyName("UsuarioGestor")]
        public string usuario { get; set; } = "";

        [JsonIgnoreAttribute]
        public string productoNombre { get; set; } = "";
    }
}
