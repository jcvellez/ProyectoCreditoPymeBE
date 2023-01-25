namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class CerrarExpedienteRequest
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0913521613</example>
        public string? Identificacion { get; set; }
        /// <summary>
        /// Producto
        /// </summary>
        /// <example>cuotaMensual</example>
        public string? Producto { get; set; }
        /// <summary>
        /// IdExpediente
        /// </summary>
        /// <example>10955946</example>
        public string? IdExpediente { get; set; }
        /// <summary>
        /// IdProceso
        /// </summary>
        /// <example>1263</example>
        public int? IdProceso { get; set; }
        /// <summary>
        /// IdCliente
        /// </summary>
        /// <example>1336</example>
        public string? IdCliente { get; set; }
        /// <summary>
        /// IdCliente
        /// </summary>
        /// <example>1</example>
        public int? Opcion { get; set; } = 1;

        public string UsuarioGestor { get; set; } = "";
        public string OpidGestor { get; set; } = "";
        public string idAgenciaOficial { get; set; }

    }


}
