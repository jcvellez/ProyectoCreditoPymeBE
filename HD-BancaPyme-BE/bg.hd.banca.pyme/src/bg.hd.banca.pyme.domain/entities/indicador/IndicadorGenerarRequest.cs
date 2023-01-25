using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.indicador
{
    public class IndicadorGenerarRequest
    {
        /// <summary>
        /// idProceso
        /// </summary>
        /// <example>25882</example>
        public int? idProceso { get; set; }
        [JsonIgnore]
        public string? usuario { get; set; }
        /// <summary>
        /// identificacion
        /// </summary>
        /// <example>0705118164</example>
        public string? identificacion { get; set; }
        [JsonIgnore]
        public string? tipoIdentificacion { get; set; }
        /// <summary>
        /// definitivo
        /// </summary>
        /// <example>S</example>
        public string? definitivo { get; set; }
         
    }
}
