using bg.hd.banca.pyme.domain.entities.NewtonsoftJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultarSectorPymesResponse
    {
        public string? CodigoRetorno { get; set; }
        public string? MensajeRetorno { get; set; }
        
        public List<Sectores>? ListaSectores { get; set; }
        public class Sectores
        {            
            public int? idSector { get; set; }            
            public string? FechaInicioActividades { get; set; }            
            public string? CodigoCiuu { get; set; }            
            public string? SectorVetado { get; set; }
        }        
    }
}
