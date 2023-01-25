using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.ClientesRatingNeo
{
    public class ClientesRatingNeoResponse
    {
        public string? codigoRetorno { get; set; }
        public string? mensaje { get; set; }
        public string? IdClienteRating { get; set; }
        public string? IdProcesoRating { get; set; }
        public string? fechaRevision { get; set; }
        //response para analisis de cliente
        public string? Informar { get; set; }
        public List<AnyosData>? anyos { get; set; }
    }
}
