using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class IngresarDetalleVentasRequest
    {

        [JsonIgnore] public string? Opcion { get; set; }
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0703691824</example>
        public string? Identificacion { get; set; }
        public List<ProductoVentas>? Productos { get; set; }

    }
    public class ProductoVentas
    {
        /// <summary>
        /// IdProductoServicio
        /// </summary>
        /// <example>2098</example>
        public int? IdProductoServicio { get; set; }
        /// <summary>
        /// Nombre
        /// </summary>
        /// <example>prueba</example>
        public string? Nombre { get; set; }
        /// <summary>
        /// Porcentaje
        /// </summary>
        /// <example>25</example>
        public int? Porcentaje { get; set; }
    }
}
