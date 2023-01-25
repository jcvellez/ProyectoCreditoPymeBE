using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva
{
    public class IngresoDeclaracionSemestralResponse
    {
        public int? CodigoRetorno { get; set; } = 0;
        [JsonProperty("mensajeRetorno")]
        public string? Mensaje { get; set; } = "";

    }
}
