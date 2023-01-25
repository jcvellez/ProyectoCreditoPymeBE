using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaResidenciaFiscalRequest
    {
        public string? opcion { get; set; } = "2";
        public string? identidad { get; set; } = "";
        [JsonIgnoreAttribute] public bool controlExcepcion { get; set; } = true;
    }
}
