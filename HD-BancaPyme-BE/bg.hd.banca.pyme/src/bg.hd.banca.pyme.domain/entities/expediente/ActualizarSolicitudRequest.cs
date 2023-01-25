using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ActualizarSolicitudRequest
    {
        /// <summary>
        /// idExpediente
        /// </summary>
        /// <example>10956018</example>
        [Required] public string idExpediente { get; set; }
        /// <summary>
        /// idProducto
        /// </summary>
        /// <example>cuotaMensual</example>
        [Required] public string idProducto { get; set; }


        public string? montoFinanciar { get; set; }
        /// <summary>
        /// tasaInteresProducto
        /// </summary>
        /// <example>10.72</example>
        public string? tasaInteresProducto { get; set; }
        /// <summary>
        /// plazo
        /// </summary>
        /// <example>24</example>
        public string? plazo { get; set; }
        /// <summary>
        /// diaPago
        /// </summary>
        /// <example>fija</example>
        public string? diaPago { get; set; }
        /// <summary>
        /// idTipoAmortizacion
        /// </summary>
        /// <example>2</example>
        public string? idTipoAmortizacion { get; set; }
        /// <summary>
        /// idSolicitud
        /// </summary>
        /// <example>229148</example>
        public string? idSolicitud { get; set; }
        /// <summary>
        /// cuota
        /// </summary>
        /// <example>null</example>
        public string? cuota { get; set; }
        /***/
    }
}