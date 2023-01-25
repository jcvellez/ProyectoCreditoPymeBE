using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.biometria
{
    public class TransaccionBiometria
    {
        public int codigoResponse { get; set; }
        public string MessageResponse { get; set; } = "";
        public string DescriptionResponse { get; set; } = "";
        public int numeroIntentosFallidos { get; set; } = 0;
        public string? tiempoBloqueoRestante { get; set; } = "0";
        public string? numeroIntentosRestantesBloqueo { get; set; } = "0";

    }
}
