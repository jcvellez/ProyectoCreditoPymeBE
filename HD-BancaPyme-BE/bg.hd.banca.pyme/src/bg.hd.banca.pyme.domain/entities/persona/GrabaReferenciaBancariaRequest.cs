using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaReferenciaBancariaRequest
    {
        [JsonIgnore] public string? Opcion { get; set; }
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0703691824</example>
        public string? Identificacion { get; set; }
        public List<Referencias>? Referencia { get; set; }
        public class Referencias
        {
            /// <summary>
            /// IdReferencia
            /// </summary>
            /// <example>1944</example>
            public int? IdReferencia { get; set; }
            /// <summary>
            /// IdBanco
            /// </summary>
            /// <example>4654</example>
            public string? IdBanco { get; set; }
            /// <summary>
            /// IdTipoCuenta
            /// </summary>
            /// <example>4593</example>
            public int? IdTipoCuenta { get; set; }
            /// <summary>
            /// NumeroCuenta
            /// </summary>
            /// <example>001245210</example>
            public string? NumeroCuenta { get; set; }
        }
    }
}
