using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.cuentas
{
    public class ConsultaCuentasRequest
    {
        [Required]
        [JsonProperty("XML")]
        public string XML { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>USRCLS01</example>
        [JsonProperty("LOGIN")]
        public string LOGIN { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>NEO</example>
        [JsonProperty("PLATAFORMA")]
        public string PLATAFORMA { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>0109</example>
        [JsonProperty("CODTRANSID")]
        public string CODTRANSID { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>C</example>
        [JsonProperty("ITIPOID")]
        public string ITIPOID { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>0917975294</example>
        [JsonProperty("IID")]
        public string IID { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>10</example>
        [JsonProperty("ITIPOPARTICIPANTE")]
        public string ITIPOPARTICIPANTE { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>99</example>
        [JsonProperty("IESTADO")]
        public string IESTADO { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>TIT</example>
        [JsonProperty("IRELACION")]
        public string IRELACION { get; set; } = "";
    }
}
