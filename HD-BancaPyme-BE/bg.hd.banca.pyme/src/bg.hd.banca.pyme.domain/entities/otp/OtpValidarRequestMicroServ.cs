using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace bg.hd.banca.pyme.domain.entities.otp
{
    public class OtpValidarRequestMicroServ
    {

        /// <summary>
        /// Parámetro llave OTP
        /// </summary>
        /// <example>elibaXA...=</example>
        [JsonIgnore]
        public string? llaveOTP { get; set; }
        /// <summary>
        /// Parámetro iv OTP
        /// </summary>
        /// <example>yC0hIx...==</example>
        [JsonIgnore]
        public string? ivOTP { get; set; }
        /// <summary>
        /// Parámetro aplicación OTP
        /// </summary>
        /// <example>facechat</example>
        [JsonIgnore]
        public string? aplicacion { get; set; }
        /// <summary>
        /// Parámetro servicio OTP
        /// </summary>
        /// <example>otp</example>
        [JsonIgnore]
        public string? servicio { get; set; }
        /// <summary>
        /// Parámetro canal OTP
        /// </summary>
        /// <example>AUTH</example>
        [JsonIgnore]
        public string? canal { get; set; }
        /// <summary>
        /// Parámetro opid OTP
        /// </summary>
        /// <example>AUTH</example>
        [JsonIgnore]
        public string? opidOTP { get; set; }
        /// <summary>
        /// Parámetro terminal OTP
        /// </summary>
        /// <example>AUTH</example>
        [JsonIgnore]
        public string? terminal { get; set; }
        /// <summary>
        /// Identificación del cliente
        /// </summary>
        /// <example>0999999999</example>
        [Required]
        public string? identificacion { get; set; }

        /// <summary>
        /// Tipo identificación del cliente
        /// </summary>
        /// <example>C</example>
        [Required]
        public string? tipoIdentificacion { get; set; }

        /// <summary>
        /// Código OTP
        /// </summary>
        /// <example>1234</example>
        [Required]
        public string otp { get; set; }

    }
}
