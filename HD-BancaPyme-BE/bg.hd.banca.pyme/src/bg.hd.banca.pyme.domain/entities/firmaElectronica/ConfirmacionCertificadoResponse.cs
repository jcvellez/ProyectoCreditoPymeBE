using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.firmaElectronica
{
    public class ConfirmacionCertificadoResponse
    {
        public string numeroSerial { get; set; } = "";
        [JsonProperty("codError")]
        public int CodigoRetorno { get; set; } = 1;
        [JsonProperty("msgError")]
        public string Mensaje { get; set; } = "";
        public bool fueraHorarioLaboral { get; set; } = false;
        public string HoraMinima { get; set; } = "";
        public string HoraMaxima { get; set; } = "";
    }
}
