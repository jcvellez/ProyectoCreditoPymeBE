using Newtonsoft.Json;

namespace bg.hd.banca.pyme.domain.entities.token.acceso
{
    public class ValidaTokenAccesoCanalApliResponse
    {
        public int CodigoRetorno { get; set; } = 1;
        [JsonProperty("mensajeRetorno")]
        public string Mensaje { get; set; } = "";
        public MetaJson metaJson { get; set; } = new MetaJson();
    }

    public class MetaJson
    {
        public string producto { get; set; } = "";
        public string identificacion { get; set; } = "";
        public string codigoDactilar { get; set; } = "";
        public string correo { get; set; } = "";
        public string canal { get; set; } = "";
        public string usuario { get; set; } = "";
        public string agencia { get; set; } = "";
        public bool flujoTokenizado { get; set; } = false;
        public string Opid { get; set; } = "";

    }
}
