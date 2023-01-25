using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.ExcepcionAnalisis
{
    public class ExcepcionAnalisisRequest
    {
        public string? Opcion { get; set; }
        public string? IdClienteRating { get; set; }
        public int? IdProceso { get; set; }
        public string? Comentario { get; set; }
        public string? Usuario { get; set; }
    }
}
