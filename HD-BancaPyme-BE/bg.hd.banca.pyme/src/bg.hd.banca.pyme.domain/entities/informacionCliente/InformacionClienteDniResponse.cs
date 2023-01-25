using bg.hd.banca.pyme.domain.entities.persona;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class InformacionClienteDniResponse
    {
        public int id { get; set; }
        public string identificacion { get; set; }
        public string nombres { get; set; }
        public string genero { get; set; }
        public string tipo { get; set; }
        public string fechaNacimiento { get; set; }
        public string lugarNacimiento { get; set; }
        public string codigoDactilar { get; set; }
        public object lugarExpedicion { get; set; }
        public string fechaExpedicion { get; set; }
        public string fechaExpiracion { get; set; }
        public string nacionalidad { get; set; }
        public string estadoCivil { get; set; }
        public string nivelEducacion { get; set; }
        public string profesion { get; set; }
        public object fechaDefuncion { get; set; }
        public object lugarDefuncion { get; set; }
        public string lugarDomicilio { get; set; }
        public string direccionDomicilio { get; set; }
        public string identificacionConyuge { get; set; }
        public bool discapacitado { get; set; }
        public bool fallecido { get; set; }
        public string idEstadoCivil { get; set; }
        public string idNacionalidad { get; set; }
        public string idGenero { get; set; }
        public string idRegimenMatrimonial { get; set; }
        public string edad  { get; set; }

        public IdentificaNombres persona = new IdentificaNombres();
    }
}
