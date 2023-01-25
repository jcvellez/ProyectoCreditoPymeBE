using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.ClientesRatingNeo
{
    public class ClientesRatingNeoRequest
    {
        /// <summary>
        /// identificacion
        /// </summary>
        /// <example>0913521613</example>
        public string? identificacion { get; set; }
        /// <summary>
        /// idExpedienteNeo
        /// </summary>
        /// <example>9819298</example>
        public string? idExpedienteNeo { get; set; }
        [JsonIgnore]
        public string? nombreCliente { get; set; }
        [JsonIgnore]
        public string? direccion { get; set; }            
    }
}
