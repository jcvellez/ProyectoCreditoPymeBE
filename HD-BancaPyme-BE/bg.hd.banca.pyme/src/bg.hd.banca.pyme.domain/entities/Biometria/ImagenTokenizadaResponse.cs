using bg.hd.banca.pyme.domain.entities.config;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.biometria
{
    public class ImagenTokenizadaResponse
    {
        public int CodigoRetorno { get; set; }
        public string MensajeRetorno { get; set; } = "";
        [JsonIgnore]
        public string PorcentajeCoincidencia { get; set; } ="";
        [JsonIgnore] 
        public string TrazabilidadNV { get; set; } = "";
        [JsonIgnore]
        public Transaccion DataTransaccion { get; set; } = new Transaccion();
        public string jwtCliente { get; set; } = "";


    }
}
