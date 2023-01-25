using System;
using System.Collections.Generic;
using System.Text;
using static bg.hd.banca.pyme.domain.entities.informacionCliente.CuentasContablesRequest;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class CuentasContablesMicroServRequest
    {
        public int? idProceso { get; set; }
        public string? idClienteRating { get; set; }
        public string? identificacion { get; set; }
        public string? tipoIdentificacion { get; set; }
        public string? usuario { get; set; }
        public string? fechaRevision { get; set; }
        public List<CuentasContables>? cuentasContables { get; set; }        
    }
}