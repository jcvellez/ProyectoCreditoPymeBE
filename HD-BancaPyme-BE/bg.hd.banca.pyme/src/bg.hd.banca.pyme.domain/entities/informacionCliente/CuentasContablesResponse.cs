using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class CuentasContablesResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        [JsonIgnore]
        public string? SaldoActivos { get; set; }
        [JsonIgnore]
        public string? SaldoPasivos { get; set; }
        [JsonIgnore]
        public string? SaldoPatrimonio { get; set; }
        [JsonIgnore]
        public string? Anyo3 { get; set; }
        [JsonIgnore]
        public string? SaldoVentaAnyo3 { get; set; }
    }
}