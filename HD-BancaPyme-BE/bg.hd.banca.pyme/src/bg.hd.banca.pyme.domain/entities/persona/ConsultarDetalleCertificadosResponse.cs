using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultarDetalleCertificadosResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        public List<DetallesCertificaciones>? certificaciones { get; set; }
    }
     
}
