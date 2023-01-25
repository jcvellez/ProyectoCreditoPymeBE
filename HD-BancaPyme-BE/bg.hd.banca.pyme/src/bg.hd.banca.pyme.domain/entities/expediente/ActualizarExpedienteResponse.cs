using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ActualizarExpedienteResponse
    {
        private Producto producto = new Producto();
        public int? CodigoRetorno { get; set; }
        public string? Mensaje { get; set; }
        [JsonIgnoreAttribute]        
        public Producto Producto { get => this.producto; set => this.producto = value; }
    }
}