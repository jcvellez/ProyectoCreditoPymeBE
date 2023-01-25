namespace bg.hd.banca.pyme.domain.entities.PreCalificador
{
    public class PreCalificadorResponse
    {
        public int? CodigoRetorno { get; set; }
        public string? Mensaje { get; set; }
        public decimal? MontoAprobado { get; set; }
        public decimal? montoSolicitado { get; set; }
        public bool tieneAprobacion { get; set; } = false;
    }
}
