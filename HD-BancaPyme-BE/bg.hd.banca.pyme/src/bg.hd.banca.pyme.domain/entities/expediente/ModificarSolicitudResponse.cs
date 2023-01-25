using bg.hd.banca.pyme.domain.entities.config;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ModificarSolicitudResponse
    {
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = "";
        [JsonIgnore]
        public Transaccion DataTransaccion { get; set; } = new Transaccion();
    }
}
