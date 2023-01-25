using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ActualizaSolicitudResponse
    {
        public int? CodigoRetorno { get; set; } = 0;
        public string? Mensaje { get; set; } = string.Empty;
    }
}