using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.BancaControl
{
    public class BancaControlResponse
    {
        public int CodigoRetorno { get; set; } = 1;
        public string Mensaje { get; set; } = "";
        public string requiereTarjetaBancontrol { get; set; } = "";
    }
}
