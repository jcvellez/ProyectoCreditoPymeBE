using System.Text.Json.Serialization;


namespace bg.hd.banca.pyme.domain.entities.PreCalificador
{
    public class PreCalificadorRequest
    {
        /// <summary>
        /// idProceso
        /// </summary>
        /// <example>43799</example>
        public int? idProceso { get; set; }
        [JsonIgnore]
        public string? tipoIdentificacion { get; set; }
        /// <summary>
        /// identificacion
        /// </summary>
        /// <example>1600288409</example>
        public string? identificacion { get; set; }
        /// <summary>
        /// idCliente
        /// </summary>
        /// <example>43196</example>
        public string? idCliente { get; set; }
        [JsonIgnore]
        public string? riesgoPropuesto { get; set; }
        /// <summary>
        /// fechaRevision
        /// </summary>
        /// <example>23-09-2022</example>
        public string? fechaRevision { get; set; }
        /// <summary>
        /// plazo
        /// </summary>
        /// <example>18</example>
        public int? plazo { get; set; }
        /// <summary>
        /// idExpediente
        /// </summary>
        /// <example>10955981</example>
        public string? idExpediente { get; set; }
        /// <summary>
        /// Producto
        /// </summary>
        /// <example>cuotaMensual</example>
        public string? Producto { get; set; }
        public int? version { get; set; } = 0;
        public int? tipoCalificacion { get; set; } = 0;
        [JsonIgnore]
        public string? fechaVecimiento { get; set; }
        [JsonIgnore]
        public string? fechaPago { get; set; }
        [JsonIgnore]
        public string? destino { get; set; }
        [JsonIgnore]
        public string? resultadoFicha { get; set; }
        /// <summary>
        /// montoSolicitado
        /// </summary>
        /// <example>2000</example>
        public decimal? montoSolicitado { get; set; }
        [JsonIgnore]
        public string? opcion { get; set; }
        [JsonIgnore]
        public int? ordenMes { get; set; }
        [JsonIgnore]
        public int? anyo { get; set; }
        [JsonIgnore]
        public double? mes { get; set; }
        [JsonIgnore]
        public string? usuario { get; set; }
        public string UsuarioGestor { get; set; } = "";
        public string OpidGestor { get; set; } = "";
    }
}