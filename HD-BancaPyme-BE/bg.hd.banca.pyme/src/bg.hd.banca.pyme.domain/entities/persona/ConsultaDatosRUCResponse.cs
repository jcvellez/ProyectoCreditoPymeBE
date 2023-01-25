using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaDatosRUCResponse
    {
        public string traceid { get; set; }
        public rsInfoRUCPersona[] data { get; set; }

    }
}
