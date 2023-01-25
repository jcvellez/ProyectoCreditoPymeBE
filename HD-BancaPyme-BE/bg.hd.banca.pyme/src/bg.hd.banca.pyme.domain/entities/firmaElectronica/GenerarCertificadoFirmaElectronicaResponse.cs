using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.firmaElectronica
{
    public class GenerarCertificadoFirmaElectronicaResponse
    {
        public string numeroSerial { get; set; } = "";
        [JsonProperty("codError")]
        public int CodigoRetorno { get; set; } = 1;
        [JsonProperty("msgError")]
        public string Mensaje { get; set; } = "";
    }
}
