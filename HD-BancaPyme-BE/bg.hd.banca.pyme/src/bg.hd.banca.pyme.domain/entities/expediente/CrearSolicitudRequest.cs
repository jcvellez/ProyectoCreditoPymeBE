using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class CrearSolicitudRequest
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0904581618</example>
        [Required]
        [StringLength(10, ErrorMessage = "La identificación debe tener 10 caracteres", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La identificación debe ser númerica")]
        public string Identificacion { get; set; }
        /// <summary>
        /// Producto
        /// </summary>
        /// <example>cuotaMensual</example>
        public string Producto { get; set; }
        /// <summary>
        /// IdSolicitud
        /// </summary>
        /// <example>226414</example>
        public int IdSolicitud { get; set; }
        [JsonIgnore]
        public string? Nombre { get; set; }

    }
}
