using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaClientesProveedoresRequest
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>1600288409</example>
        public string? Identificacion { get; set; }
        /// <summary>
        /// TipoClienteProveedor
        /// </summary>
        /// <example>2</example>
        public int? TipoClienteProveedor { get; set; } = 0;
        public List<DetallesPersonas>? Personas { get; set; }

    }
    public class DetallesPersonas
    {
        /// <summary>
        /// idPersona
        /// </summary>
        /// <example>47</example>
        public int idPersona { get; set; } = 0;
        /// <summary>
        /// idPersona
        /// </summary>
        /// <example>cliente 2</example>
        public string? nombre { get; set; }
        /// <summary>
        /// porcentaje
        /// </summary>
        /// <example>cliente 0</example>
        public string? porcentaje { get; set; }
        /// <summary>
        /// diasCredito
        /// </summary>
        /// <example>12178</example>        
        public string? diasCredito { get; set; }
    }
}
