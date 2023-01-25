using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.crmCasos
{
    public class ActualizaInformacionCrmRequest
    {
        [JsonIgnoreAttribute]public string tipoIdentificacion { get; set; } = "C";
        /// <summary>
        /// Identificación del cliente
        /// </summary>
        /// <example>0999999999</example>
        public string identificacion { get; set; } = "";
        [JsonIgnoreAttribute] public string canal { get; set; } = "";
        /// <summary>
        /// Usuario
        /// </summary>
        /// <example>USR</example>
        public string usuario { get; set; } = "";
        /// <summary>
        /// Total Activos
        /// </summary>
        /// <example>888.88</example>
        public string totalActivos { get; set; } = "";
        /// <summary>
        /// Total Pasivos
        /// </summary>
        /// <example>222.22</example>
        public string totalPasivos { get; set; } = "";
        /// <summary>
        /// Total Patrimonio
        /// </summary>
        /// <example>666.66</example>
        public string totalPatrimonio { get; set; } = "";
        /// <summary>
        /// Fecha Patrimonio
        /// </summary>
        /// <example>2022-10-13</example>
        public string fechaPatrimonio { get; set; } = "";
        [JsonIgnoreAttribute] public bool controlExcepcion { get; set; } = false;
    }
}
