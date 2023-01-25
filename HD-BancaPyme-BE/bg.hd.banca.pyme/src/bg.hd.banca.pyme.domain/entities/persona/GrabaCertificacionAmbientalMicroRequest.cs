using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabaCertificacionAmbientalMicroRequest
    {

        public string? Opcion { get; set; }
        public string? Identificacion { get; set; }        
        public List<DetallesCertificaciones>? Certificaciones { get; set; }
    }
}
