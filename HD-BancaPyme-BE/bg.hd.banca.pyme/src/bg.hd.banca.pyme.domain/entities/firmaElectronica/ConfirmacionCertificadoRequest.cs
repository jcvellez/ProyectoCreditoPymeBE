using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.firmaElectronica
{
    public class ConfirmacionCertificadoRequest
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
        public string? IdExpediente { get; set; } = "";
        [JsonPropertyName("UsuarioGestor")]
        public string usuario { get; set; } = "";
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0917398190</example>
        [Required]
        [StringLength(10, ErrorMessage = "La identificación debe tener 10 caracteres", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La identificación debe ser númerica")]
        public string identificacion { get; set; } = "";
        /// <summary>
        /// Clave o firma que le llega al correo o sms al cliente
        /// </summary>
        /// <example>wfvfvfvj0j9817989</example>
        [Required]
        [JsonPropertyName("codigoFirma")]
        public string? clave { get; set; } = "";
        /// <summary>
        /// Ip del cliente
        /// </summary>
        /// <example>192.168.12.3</example>
        [JsonIgnoreAttribute]
        public string? ip { get; set; } = "";
    }
}
