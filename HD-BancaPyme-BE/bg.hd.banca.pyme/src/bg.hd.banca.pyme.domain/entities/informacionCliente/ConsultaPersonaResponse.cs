using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class ConsultaPersonaResponse
    {
        [JsonIgnore] public int CODIGO { get; set; }
        [JsonIgnore] public string MENSAJE { get; set; }
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        public DatosPersonaResponse DatosPersona { get; set; }
        public DatosPersonaResponse DatosConyugue { get; set; }
    }
}
