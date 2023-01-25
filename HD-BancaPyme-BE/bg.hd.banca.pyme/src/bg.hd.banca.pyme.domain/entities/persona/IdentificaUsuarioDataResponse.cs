using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class IdentificaUsuarioDataResponse
    {
        public int id { get; set; }
        public string identificacion { get; set; }
        public string tipoIdentificacion { get; set; }
        public string nombres { get; set; }
        public string direccion1 { get; set; }
        public string telefono1 { get; set; }
        public string correoElectronico { get; set; }
        public string telefonoCelular { get; set; }
        public string segmentoEstrategico { get; set; }
        public string calificacion { get; set; }
        public string estado { get; set; }
        public string sexo { get; set; }
        public string fechaNacimiento { get; set; }
        public string oficial { get; set; }
    }
}
