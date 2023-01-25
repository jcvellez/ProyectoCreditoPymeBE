using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva
{
    public class IngresoDeclaracionSemestralRequest
    {
        [JsonIgnore]
        public string? opcion { get; set; } = "UPD";
        /// <summary>
        /// </summary>
        /// <example>0</example>
        public string? idProceso { get; set; } = "0";
        /// <summary>
        /// </summary>
        /// <example>250000</example>
        public string? mesSaldo { get; set; } = "0";
        /// <summary>
        /// </summary>
        /// <example>50000</example>
        public string? mesGasto { get; set; } = "0";
        [JsonIgnore]
        public string? tipo { get; set; } = "3";

    }
}
