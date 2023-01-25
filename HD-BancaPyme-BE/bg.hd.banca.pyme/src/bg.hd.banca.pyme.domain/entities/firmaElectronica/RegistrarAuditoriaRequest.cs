using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.firmaElectronica
{
    public class RegistrarAuditoriaRequest
{       /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0917398190</example> 
        public string identificacion { get; set; } = "";
        [JsonIgnoreAttribute] public string opcion { get; set; } = "";
        /// <summary>
        /// IdExpediente
        /// </summary>
        /// <example>9819298</example>
        public string idExpediente { get; set; } = "";
        /// <summary>
        /// Accion
        /// </summary>
        /// <example>ver-documento</example>
        public string accion { get; set; } = "";
        [JsonIgnoreAttribute] public string? idAccion { get; set; } = "";
        /// <summary>
        /// IDDocumento
        /// </summary>
        /// <example>09</example>
        public string idDocumento { get; set; } = "";
        /// <summary>
        /// Usuario
        /// </summary>
        /// <example>HCHOVENGO</example>
        [JsonPropertyName("UsuarioGestor")]
        public string usuario { get; set; } = "";
        [JsonIgnoreAttribute] public string ip { get; set; } = "";
        /// <summary>
        /// Usuario
        /// </summary>
        /// <example>Certificado generado con éxito</example>
        [JsonIgnoreAttribute] public string comentario { get; set; } = "";
    }
}
