using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.ArchivosImpuesto
{
    public class ArchivosImpuestoResponse
    {
        public int CodigoRetorno { get; set; }
        public string mensaje { get; set; }
        [JsonIgnore]
        public string? BanActVentas { get; set; }
        [JsonIgnore]
        public string? VentasAnuales { get; set; }
        [JsonIgnore]
        public string? SaldoActivos { get; set; }
        [JsonIgnore]
        public string? SaldoPasivos { get; set; }
        [JsonIgnore]
        public string? SaldoPatrimonio { get; set; }
    }
}
