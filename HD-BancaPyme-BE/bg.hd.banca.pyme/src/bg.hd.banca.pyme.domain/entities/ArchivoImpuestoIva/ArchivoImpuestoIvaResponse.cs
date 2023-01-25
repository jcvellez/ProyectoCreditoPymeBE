using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva
{
    public class ArchivoImpuestoIvaResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        [JsonIgnore]
        public int CodError { get; set; }
    }
}
