using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.catalogo
{
    public class ConsultarCatalogoResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        public ListaCatalogoDetalle listaCatalogoDetalle { get; set; }
    }

    public class ListaCatalogoDetalle
    {
        public List<CatalogoDetalle> catalogoDetalle { get; set; }
    }
    public class CatalogoDetalle
    {
        public int idCodigo { get; set; }
        //public string strDescripcion { get; set; }
        public string strValor { get; set; }
        //public int idCodigoPadre { get; set; }
        [JsonIgnore]
        public string strCodigoHost { get; set; }
        //public string strCodigoCRM { get; set; }
        //public string strValorHost { get; set; }
        //public string strCodigoSBS { get; set; }
        public string strValor2 { get; set; }
        //public string strValor3 { get; set; }
        [JsonIgnore]
        public string strValor4 { get; set; }
        [JsonIgnore]
        public string strValor5 { get; set; }
    }
}