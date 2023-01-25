using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultarExpProductoActRequest
    {
        /// <summary>
        /// Producto
        /// </summary>
        /// <example>credito-condicionado</example>
        [Required]
        public string descriptionProducto { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>9817378</example>
        [Required]
        public int? idExpediente { get; set; } = 0;
        /// <summary>
        /// </summary>
        /// <example>1</example>        
        public string? idProductoPadre { get; set; } = "0";
        /// <summary>
        /// </summary>
        /// <example>2216</example>
        public string? idProducto { get; set; } = "0"; 
        /// <summary>
        /// </summary>
        /// <example>neoWeb</example>
        public string? usuario { get; set; } = "";

    }
}
