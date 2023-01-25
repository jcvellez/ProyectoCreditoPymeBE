using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultarDetalleVentasResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        public List<DetallesVenta>? informacionVentasProductos { get; set; }
    }
    public class DetallesVenta
    {
        public int? IdProductoServicio { get; set; }
        public string? Nombre { get; set; }
        public string? Porcentaje { get; set; }
    }
}
