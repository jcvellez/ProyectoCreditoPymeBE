using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.firmaElectronica
{
    public class RegistrarAuditoriaResponse
    {        
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = "";
        [JsonIgnore] public Estado ESTATUS { get; set; } = new Estado();
        [JsonIgnore] public Mensajes DataSet { get; set; } = new Mensajes();
        

        public class Estado
        {
            public int CODIGO { get; set; } = 0;
            public string MENSAJE { get; set; } = "";
        }

        public class Mensajes
        {
            public DetalleMensaje Table { get; set; } = new DetalleMensaje();

        }

        public class DetalleMensaje
        {
            public string idCodigo { get; set; } = "";
            public string Mensaje { get; set; } = "";
        }
    }
}