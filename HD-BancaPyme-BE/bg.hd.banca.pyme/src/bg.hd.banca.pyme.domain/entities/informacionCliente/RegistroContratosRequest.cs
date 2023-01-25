using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class RegistroContratosRequest
    {
        /// <example>0927259630</example>
        public string? identificacion { get; set; }
        /// <example>true</example>
        public bool? esCliente { get; set; }
        /// <example>IVR</example>
        public string? canal { get; set; }
        /// <example>Dispositivo</example>
        public string? dispositivo { get; set; }
        public List<Consentimientos> consentimientos { get; set; }
        /// <example>userbg</example>
        public string? usuario { get; set; }
        /// <example>1</example>
        public int? codAgencia { get; set; }
        /// <example>127.0.0.1</example>
        public string? ip { get; set; }
    }

    public class Consentimientos
    {
        /// <example>CTC0001</example>
        public string? numReferencia { get; set; }
        /// <example>true</example>
        public bool? consentimientoDelTitular { get; set; }
        /// <example></example>
        public string? numReferenciaSolicitud { get; set; }
    }
}
