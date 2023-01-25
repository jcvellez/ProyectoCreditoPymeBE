using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class GuardarDatosNegocioRequest
    {
        /// <summary>
        /// numIdentificacion
        /// </summary>
        /// <example>1002143772</example>
        public string? identificacion { get; set; }
        /// <summary>
        /// idCodigoNeoCIUU
        /// </summary>
        /// <example>37925</example>
        public string? idCodigoNeoCIUU { get; set; }
        /// <summary>
        /// num_empleados
        /// </summary>
        /// <example>55</example>
        public int? num_empleados { get; set; }
        /// <summary>
        /// inmueble_propio
        /// </summary>
        /// <example>1</example>
        public int? inmueble_propio { get; set; }
        /// <summary>
        /// telefono
        /// </summary>
        /// <example>0888888888</example>
        public string? telefono { get; set; }
        /// <summary>
        /// idParroquia
        /// </summary>
        /// <example>3187</example>
        public int? idParroquia { get; set; }
        /// <summary>
        /// idProvincia
        /// </summary>
        /// <example>18</example>
        public string? idProvincia { get; set; }
        /// <summary>
        /// idCiudad
        /// </summary>
        /// <example>56</example>
        public string? idCiudad { get; set; }
        /// <summary>
        /// direccion
        /// </summary>
        /// <example>Urb DreamGod III</example>
        public string? direccion { get; set; }
        /// <summary>
        /// compania_aseguradora
        /// </summary>
        /// <example>4892</example>
        public int? compania_aseguradora { get; set; }
        /// <summary>
        /// Referencia
        /// </summary>
        /// <example>aqui la referencia</example>
        public string? Referencia { get; set; }
        public string? Producto { get; set; } = "";
        public string? idSolicitud { get; set; } = "";
        public string? tieneActDirNegocio { get; set; } = "";

    }
}