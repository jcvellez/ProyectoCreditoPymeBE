using bg.hd.banca.pyme.domain.entities.persona;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultaDatosNegocioResponse
    {
        public int CodigoRetorno { get; set; }
        public string? Mensaje { get; set; }
        public string? idPersona { get; set; }
        public string? telefono { get; set; }
        //Direcion
        public string? idProvincia { get; set; }
        public string? Provincia { get; set; }
        //public string idCiudad { get; set; }
        public string? idCiudad { get; set; }
        public string? Ciudad { get; set; }
        public string? Direccion { get; set; }
        public string? Referencia { get; set; }
        public string? actividadEconomica { get; set; }
        public string? codigoCIUU { get; set; }
        public string idParroquia { get; set; }
        public string Parroquia { get; set; }
        public string? idCodigoNeoCIUU { get; set; }
        public string inmueble_propio { get; set; }
        public string compania_aseguradora { get; set; }
        public int NumeroEmpleados { get; set; }
        public bool tieneSeguro { get; set; } = false;
    }
}