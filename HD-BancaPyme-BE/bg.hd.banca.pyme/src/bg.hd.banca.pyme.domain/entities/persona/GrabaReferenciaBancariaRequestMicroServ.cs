using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static bg.hd.banca.pyme.domain.entities.persona.GrabaReferenciaBancariaRequest;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaReferenciaBancariaRequestMicroServ
    {
        [JsonIgnore] public string? Opcion { get; set; }
        public string? Identificacion { get; set; }
        public List<Referencias>? Referencias { get; set; }
    }
}
