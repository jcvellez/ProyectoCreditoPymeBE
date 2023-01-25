using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaCertificacionAmbientalRequest
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>1600288409</example>
        public string? Identificacion { get; set; }       
        public List<DetallesCertificaciones>? certificaciones { get; set; }

    }
    public class DetallesCertificaciones
    {
        /// <summary>
        /// idCatDetCertificado
        /// </summary>
        /// <example>1</example>
        public string? idCatDetCertificado { get; set; }
        /// <summary>
        /// descripcion
        /// </summary>
        /// <example>descripcion de prueba</example>
        public string? descripcion { get; set; }
    }
}