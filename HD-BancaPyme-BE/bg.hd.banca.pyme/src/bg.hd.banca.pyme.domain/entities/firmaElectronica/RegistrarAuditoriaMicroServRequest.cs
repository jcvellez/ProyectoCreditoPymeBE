using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.firmaElectronica
{
    public class RegistrarAuditoriaMicroServRequest
    {
        public string opcion { get; set; } = "";
        public string idExpediente { get; set; } = "";
        public string idAccion { get; set; } = "";
        public string idDocumento { get; set; } = "";
        public string usuario { get; set; } = "";
        public string ip { get; set; } = "";
        public string comentario { get; set; } = "";
    }
}
