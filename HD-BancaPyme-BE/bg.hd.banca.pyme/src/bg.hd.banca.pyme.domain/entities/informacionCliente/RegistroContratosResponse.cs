using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class RegistroContratosResponse
    {
        [JsonIgnore] public int codigoRetorno { get; set; }
        [JsonIgnore] public string mensajeRetorno { get; set; }
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
    }
}
