using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class PersonaNombre
    {
        string nombre1 = string.Empty;
        string nombre2 = string.Empty;
        string apellido1 = string.Empty;
        string apellido2 = string.Empty;

        public string Nombre1 { get => nombre1; set => nombre1 = value; }
        public string Nombre2 { get => nombre2; set => nombre2 = value; }
        public string Apellido1 { get => apellido1; set => apellido1 = value; }
        public string Apellido2 { get => apellido2; set => apellido2 = value; }
    }
}
