using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class InformacionClienteResponse
    {
        public string? Identificacion { get; set; }

        public string? nombres { get; set; }

        public string primerNombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string SegundoApellido { get; set; }


        public string telefonoCelular { get; set; }

        public string correoElectronico { get; set; }

        public string nacionalidad { get; set; }
        public string estadoCivil { get; set; }
        public string fechaNacimiento { get; set; }

        public string provincia { get; set; }
        public string ciudad { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }       
    }
}
