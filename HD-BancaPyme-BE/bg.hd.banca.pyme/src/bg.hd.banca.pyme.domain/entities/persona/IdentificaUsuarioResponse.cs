using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class IdentificaUsuarioResponse
    {
        public string? PrimerNombre { get; set; }
        public string? CelularOfuscado { get; set; }
        public string? correoElectronico { get; set; }
        public bool? tieneProducto { get; set; } = false;
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        public string idEtapa { get; set; }
        public string estadoExpediente { get; set; }
        public string etapaExpediente { get; set; }
        public bool envioBiometria { get; set; } = false;
        public string? codigoDactilar { get; set; } 
        public string UsuarioGestor { get; set; } = "";
        public string OpidGestor { get; set; } = "";
        public string idAgenciaOficial { get; set; }
    }
}