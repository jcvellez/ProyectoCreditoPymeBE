using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.catalogo
{
    public class ConsultarCatalogoRequest
    {
        /// <summary>
        /// Producto 
        /// </summary>
        /// <example>cuotaMensual</example>
        [Required] public string producto { get; set; }
        /// <summary>
        /// Opción 
        /// </summary>
        /// <example>1</example>
        [Required] public int opcion { get; set; }
        /// <summary>
        /// IdCatalogo 
        /// </summary>
        /// <example>47</example>
        [Required] public string idCatalogo { get; set; }
        /// <summary>
        /// IdCatalogoPadre
        /// </summary>
        /// <example>0</example>
        public string? idCatalogoPadre { get; set; }
        [JsonIgnore]
        public string? Filtro { get; set; }
        [JsonIgnore]
        public string? valorFiltro { get; set; }

       
    }
}
