using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ActualizaVentasClienteMicroServ
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
        [JsonProperty("TIPOID")]
        public string TIPOID { get; set; } = "";
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>0917975294</example>
        [JsonProperty("ID")]
        public string ID { get; set; } = "";
    
        /// <summary>
        /// bandera para actualizar clientes vtas
        /// </summary>
        /// <example>S</example>
        [JsonProperty("UPCLVTAS")]        
        public string UPCLVTAS { get; set; } = "";
        /// <summary>
        /// Valor vtas clientes
        /// </summary>
        /// <example>100000</example>
        [JsonProperty("CLVTAS")]        
        public string CLVTAS { get; set; } = "";
        /// <summary>
        /// bandera para actualizar fecha vtas
        /// </summary>
        /// <example>S</example>
        [JsonProperty("UPCLFVTANUAL")]
        //public string UPCLFVTANUAL { get => _UPCLFVTANUAL; set => _UPCLFVTANUAL = value; }
        public string UPCLFVTANUAL { get; set; } = "";
        /// <summary>
        /// fecha estado impuesto renta de vtas
        /// </summary>
        /// <example>20221231</example>
        [JsonProperty("CLFVTANUAL")]
        public string CLFVTANUAL { get; set; } = "";
        //public string CLFVTANUAL { get => _CLFVTANUAL; set => _CLFVTANUAL = value; }
    }
}
