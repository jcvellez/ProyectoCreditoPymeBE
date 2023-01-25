using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static bg.hd.banca.pyme.domain.entities.persona.GrabaReferenciaBancariaRequest;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaReferenciaBancariaResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        [JsonIgnore]public List<Referencias>? InformacionRerenciasBancarias { get; set; }
    }
}
