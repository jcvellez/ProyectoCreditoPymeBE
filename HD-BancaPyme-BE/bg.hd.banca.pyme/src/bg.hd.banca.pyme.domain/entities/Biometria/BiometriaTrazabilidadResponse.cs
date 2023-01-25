using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.biometria
{
    public class BiometriaTrazabilidadResponse
    {
        public int idTrazabilidad { get; set; } = 0;
        public string fecha { get; set; } = "";
        public string identificacion { get; set; } = "";
        public string dictamen { get; set; } = "";
        public string detalle { get; set; } = "";
        public int porcentajeCoincidencia { get; set; } = 0;
        public string fotoTomada { get; set; } = "";
        public string nombres { get; set; } = "";

    }
}
