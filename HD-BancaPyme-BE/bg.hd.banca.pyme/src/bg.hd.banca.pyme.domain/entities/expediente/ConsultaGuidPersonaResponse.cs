using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultaGuidPersonaResponse
    {
        public string? guid_persona { get; set; }
        [JsonIgnore] public string? id_persona { get; set; }
        [JsonIgnore] public int? id_clte { get; set; }
        [JsonIgnore] public string? nombre { get; set; }
        [JsonIgnore] public string? genero { get; set; }
        [JsonIgnore] public string? ciudadania { get; set; }
        [JsonIgnore] public string? rg_estado_civil { get; set; }
    }
}
