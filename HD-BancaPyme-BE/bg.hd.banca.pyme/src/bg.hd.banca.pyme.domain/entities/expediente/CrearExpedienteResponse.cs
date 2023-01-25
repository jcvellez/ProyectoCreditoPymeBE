using bg.hd.banca.pyme.domain.entities.config;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class CrearExpedienteResponse
    {


        string _idExpediente = string.Empty;
        string _usuario = string.Empty;
        int codigoRetorno = 0;
        string mensaje = string.Empty;
        Transaccion dataTransaccion = new Transaccion();
        

        public string idExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string usuario { get => _usuario; set => _usuario = value; }
        public int CodigoRetorno { get => codigoRetorno; set => codigoRetorno = value; }
        public string Mensaje { get => mensaje; set => mensaje = value; }
        [JsonIgnore] public Transaccion DataTransaccion { get => dataTransaccion; set => dataTransaccion = value; }
    }
}
