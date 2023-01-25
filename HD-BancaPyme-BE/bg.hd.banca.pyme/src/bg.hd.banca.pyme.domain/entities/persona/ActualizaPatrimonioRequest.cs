using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona

{
    public class ActualizaPatrimonioRequest
    {
        public string numIdentificacion { get; set; } = "";
        [JsonIgnoreAttribute] public string canal { get; set; } = "";

        public string usuario { get; set; } = "";

        public string activos { get; set; } = "";

        public string pasivos { get; set; } = "";

        public string patrimonio { get; set; } = "";

        public string tipoTransaccion { get; set; } = "";

    }
}
