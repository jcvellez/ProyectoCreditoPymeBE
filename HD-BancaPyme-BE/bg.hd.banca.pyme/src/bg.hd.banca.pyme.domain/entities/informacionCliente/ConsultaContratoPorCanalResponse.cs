using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class ConsultaContratoPorCanalResponse
    {
        //public List<ConsultaContratoPorCanalResponse> lista { get; set; }
        public Contrato contrato { get; set; }
        public ContratoFirmado contratoFirmado { get; set; }
    }

    public class Contrato
    {
        public string? numReferencia { get; set; }
        public string? descripcion { get; set; }
        public string? mesesValidez { get; set; }
        public string? archivoLink { get; set; }
        public bool? requerido { get; set; }
        public string? fechaContrato { get; set; }
    }

    public class ContratoFirmado
    {
        public string? numReferencia { get; set; }
        public string? descripcion { get; set; }
        public bool? consentimientoDelTitular { get; set; }
        public string? fechaExpiracion { get; set; }
        public string? fechaHoraAceptacion { get; set; }       
    }
}
