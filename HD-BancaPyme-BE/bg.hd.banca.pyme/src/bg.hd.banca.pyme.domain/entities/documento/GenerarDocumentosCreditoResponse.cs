using bg.hd.banca.pyme.domain.entities.NewtonsoftJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.documento
{
    public class GenerarDocumentosCreditoResponse
    {
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = "";
        [JsonProperty("documentosFirmaElectronica")]
        [JsonConverter(typeof(SingleValueArrayConverter<DocumentosFirmaElectronica>))]
        public List<DocumentosFirmaElectronica> documentosFirmaElectronica { get; set; } = new List<DocumentosFirmaElectronica>();
    }
}
