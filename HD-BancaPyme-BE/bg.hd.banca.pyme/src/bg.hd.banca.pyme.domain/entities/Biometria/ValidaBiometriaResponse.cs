using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.biometria
{
    public class ValidaBiometriaResponse
    {
        public int CodigoRetorno { get; set; } = 1;
        public string Mensaje { get; set; } = "";
        public int numeroIntentosFallidos { get; set; } = 0;
        public string fechaBloqueoIntentosFallidos { get; set; } = "";
        public string? tiempoBloqueoRestante { get; set; } = "0";
    }
}
