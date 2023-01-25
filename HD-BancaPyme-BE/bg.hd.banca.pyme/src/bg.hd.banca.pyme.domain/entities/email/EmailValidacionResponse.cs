using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.email
{
    public class EmailValidacionResponse
    {
        public string? descripcionValidacion { get; set; }
        public string? estadoValidacion { get; set; }
        public string? fechaValidacion { get; set; }
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
    }
}
