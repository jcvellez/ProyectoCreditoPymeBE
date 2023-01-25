using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class EncolarExpProcesoNeoBatchRequest
    {
        /// <summary>
        /// </summary>
        /// <example>1</example>
        [Required]        
        public string? opcion { get; set; } = "1"; // opción 1 - insertar el proceso
        /// <summary>
        /// </summary>
        /// <example>9817378</example>
        [Required]       
        public string? idExpediente { get; set; } = "0";
        /// <summary>
        /// </summary>
        /// <example>0</example>        
        public string idCodigo { get; set; } = "0";// id de registro en la tabla de cola de procesos batch
        /// <summary>
        /// </summary>
        /// <example>13</example>
        // idTipoProceso:13 --> Encolar Indexacion documento FOTO 
        // idTipoProceso:14 --> Encolar Indexacion documento IDENTIDAD         
        public string idTipoProceso { get; set; } = ""; //indexar id (13 y 14) - (documento FOTO y documento IDENTIDAD)
        /// <summary>
        /// </summary>
        /// <example>1</example>
        public string idEstadoProceso { get; set; } = "1";// id de estado de registro : 1-Pendiente
        /// <summary>
        /// </summary>
        /// <example>Inicia Proceso de Indexacion documento FOTO</example>
        // idTipoProceso:13 --> comentarioEstadoProceso: Inicia Proceso de Indexacion documento FOTO 
        // idTipoProceso:14 --> comentarioEstadoProceso: Inicia Proceso de Indexacion documento IDENTIDAD
        public string comentarioEstadoProceso { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>9817378</example>
        public string usuarioInvoca { get; set; } = "";
        /// <summary>
        /// </summary>
        /// <example>26/08/2022</example>
        public string fechaGenerarAprobarSolHost { get; set; } = "";
        [JsonIgnoreAttribute] public bool controlExcepcion { get; set; } = true;
    }
}
