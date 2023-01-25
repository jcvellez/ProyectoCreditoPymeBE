using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.ms
{
    public class MsClienteInformacion
    {
        public string? nombreCliente { get; set; }
        public string? correoElectronico { get; set; }
        public string? telefonoCelular { set; get; }
    }
}
