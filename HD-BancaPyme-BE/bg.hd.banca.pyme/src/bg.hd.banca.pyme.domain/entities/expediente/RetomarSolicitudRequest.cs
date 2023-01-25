using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class RetomarSolicitudRequest
    {
        string _identificacion = string.Empty;
        string _producto = string.Empty;
        string _opcion = string.Empty;
        string _idCanal = string.Empty;
        string _idProducto = string.Empty;
        /// <summary>
        /// numIdentificacion
        /// </summary>
        /// <example>0913521613</example>
        public string numIdentificacion { get => _identificacion; set => _identificacion = value; }
        [JsonIgnore]

        public string producto { get => _producto; set => _producto = value; }
        [JsonIgnoreAttribute] public string opcion { get => _opcion; set => _opcion = value; }
        [JsonIgnoreAttribute] public string idCanal { get => _idCanal; set => _idCanal = value; }
        [JsonIgnoreAttribute] public string idProducto { get => _idProducto; set => _idProducto = value; }
        public bool ClienteTokenizado { get; set; } = false;

    }
}
