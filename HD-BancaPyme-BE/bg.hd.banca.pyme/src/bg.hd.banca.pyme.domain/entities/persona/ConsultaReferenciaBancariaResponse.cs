using bg.hd.banca.pyme.domain.entities.NewtonsoftJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static bg.hd.banca.pyme.domain.entities.persona.GrabaReferenciaBancariaRequest;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaReferenciaBancariaResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }                        
        public List<Referencias>? InformacionRerenciasBancarias { get; set; }
    }
}
