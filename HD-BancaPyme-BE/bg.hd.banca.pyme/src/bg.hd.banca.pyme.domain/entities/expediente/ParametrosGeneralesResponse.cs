using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ParametrosGeneralesResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }

        [JsonIgnore] public Estado ESTATUS { get; set; }
        public ProductoPG DataSet { get; set; }

        public ParametrosGeneralesResponse()
        {
            ESTATUS = new Estado();
            DataSet = new ProductoPG();
        }
    }
    public class ProductoPG
    {
        public List<DetalleProductoPG> Table { get; set; }
        [JsonIgnore] public Mensaje Table1 { get; set; }

        public ProductoPG()
        {
            Table = new List<DetalleProductoPG>();
            Table1 = new Mensaje();
        }

    }

    public class DetalleProductoPG
    {
        public string idCodigo { get; set; }
        public string idProducto { get; set; }

        public string nombreCampo { get; set; }

        public string nombreFisico { get; set; }

        public string valorCampo { get; set; }

        public string idTipoDato { get; set; }

        public string idTipoParametro { get; set; }

        public string idSistema { get; set; }

        public string idEstado { get; set; }

        public string usuarioCrea { get; set; }

        public string fechaCrea { get; set; }

    }

    public class Mensaje
    {
        public string idCodigo { get; set; }
        public string msgError { get; set; }
    }
    public class Estado
    {
        public int CODIGO { get; set; }
        public string? MENSAJE { get; set; }
    }
}
