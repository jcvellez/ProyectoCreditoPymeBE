using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultaDatosNegocioRequest
    {
        [Required]
        [StringLength(10, ErrorMessage = "La identificación debe tener 10 caracteres", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La identificación debe ser númerica")]
        public string identificacion { get; set; }

        //public string? idActividadCiiu { get; set; }
    }
}
