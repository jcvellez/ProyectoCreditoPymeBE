using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class IngresarDetalleVentasRequestMicroServ
    {
        [JsonIgnore] public string? Opcion { get; set; }
        public string? Identificacion { get; set; }
        public List<ProductoVentas>? Productos { get; set; }
    }
}