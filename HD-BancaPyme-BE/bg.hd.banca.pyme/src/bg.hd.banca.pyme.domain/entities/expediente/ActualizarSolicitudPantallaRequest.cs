using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ActualizarSolicitudPantallaRequest
    {
        /// <summary>
        /// Producto
        /// </summary>
        /// <example>cuotaMensual</example>
        public string? Producto { get; set; }
        /// <summary>
        /// IdSolicitud
        /// </summary>
        /// <example>45908</example>
        public string? IdSolicitud { get; set; }
        /// <summary>
        /// UrlVista
        /// </summary>
        /// <example>pagina_de_prueba</example>
        public string? UrlVista { get; set; }
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0913521613</example>
        public string? Identificacion { get; set; }
    }
}
