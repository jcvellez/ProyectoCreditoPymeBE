using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.biometria
{
    public class ValidaBiometriaRequest
    {
        public string Producto { get; set; } = "";
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La identificación debe ser númerica")]
        [StringLength(10, ErrorMessage = "La identificación debe tener 10 caracteres", MinimumLength = 10)]
        public string Identificacion { get; set; } = "";
        public string ? idProducto { get; set; } = "0";
    }
}
