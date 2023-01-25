using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaReferenciaBancariaRequestMicroServ
    {
        [JsonIgnore] public string? Opcion { get; set; }
        public string? Identificacion { get; set; }        
    }
}
