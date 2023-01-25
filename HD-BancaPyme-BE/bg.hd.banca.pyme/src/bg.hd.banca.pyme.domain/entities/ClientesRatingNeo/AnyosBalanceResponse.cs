using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.ClientesRatingNeo
{
    public class AnyosBalanceResponse
    {
        public int? codigoRetorno { get; set; }
        public string? mensaje { get; set; }
        public string? Informar { get; set; }
        public List<AnyosData>? anyos { get; set; }
    }
}
