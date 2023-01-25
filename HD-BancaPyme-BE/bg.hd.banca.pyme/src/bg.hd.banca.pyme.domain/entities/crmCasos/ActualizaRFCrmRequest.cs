using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.crmCasos
{
    public class ActualizaRFCrmRequest
    {
        [JsonIgnoreAttribute] public string tipoIdentificacion { get; set; } = "C";
        /// <summary>
        /// Identificación del cliente
        /// </summary>
        /// <example>0999999999</example>
        public string identificacion { get; set; } = "";
        [JsonIgnoreAttribute] public string canal { get; set; } = "";
        /// <summary>
        /// Usuario
        /// </summary>
        /// <example>USR</example>
        public string usuario { get; set; } = "";
        public Direcciones direcciones { get; set; } = new Direcciones();
        [JsonIgnoreAttribute] public bool controlExcepcion { get; set; } = false;
    }

    public class Direcciones
    {
        public List<DireccionFiscal> direccionFiscal { get; set; } = new List<DireccionFiscal>();
    }

    public class DireccionFiscal
    {
        /// <summary>
        /// Otro Pais
        /// </summary>
        /// <example>1</example>
        public string OtroPais { get; set; } = "";
        /// <summary>
        /// Pais
        /// </summary>
        /// <example>EC</example>
        public string Pais { get; set; } = "";
        /// <summary>
        /// Numero NIF
        /// </summary>
        /// <example>56789</example>
        public string NumeroNIF { get; set; } = "";
        /// <summary>
        /// Direccion
        /// </summary>
        /// <example>CDLA MIRAFLORES</example>
        public string Direccion { get; set; } = "";
    }
}

