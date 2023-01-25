using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.documento
{ 
    public class DocumentosFirmaElectronica
    {
        [JsonProperty("idDocumentoNeo")]
        public int idDocumentoNeo { get; set; } = 0;
        [JsonProperty("idDocumentoOnbase")]
        public string idDocumentoOnbase { get; set; } = "";
        [JsonProperty("nombreDocumentoOnbase")]
        public string nombreDocumentoOnbase { get; set; } = "";
        [JsonProperty("nombreDocumento")]
        public string nombreDocumento { get; set; } = "";
        [JsonProperty("tipoDocumento")]
        public string tipoDocumento { get; set; } = "";
        [JsonProperty("objDocumento")]
        public string objDocumento { get; set; } = "";
    }
}
