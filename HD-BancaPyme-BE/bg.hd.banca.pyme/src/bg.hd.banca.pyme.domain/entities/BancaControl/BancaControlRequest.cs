using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.BancaControl
{
    public class BancaControlRequest
    {
        /// <summary>
        /// </summary>
        /// <example>0916187248</example>
        [Required]
        public string? usuario { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>BGGRUPO/example>
        [Required]
        public string? usuariowindows { get; set; } = "";
        /// <summary>
        /// '1', 47, 0, '', ''
        /// </summary>
        /// <example>NEO</example>
        [JsonIgnoreAttribute]
        public string? canal { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>JYZ</example>
        public string? opid { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>01QL</example>
        [JsonIgnoreAttribute]
        public string? terminal { get; set; } = "";
    }
}
