using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class rsConsultaRUCPersona
    {

        public bool success { get; set; }
        public cErrores error { get; set; }
        public string traceId { get; set; }
        public bool collection { get; set; }
        public int count { get; set; }
        public rsInfoRUCPersona[] data { get; set; }       

    }

    public class cErrores
    {
        public int status { set; get; }
        public string errorCode { set; get; }
        public string userMessage { set; get; }
    }

    public class rsInfoRUCPersona
    {
        public string numero { get; set; }
        public string identificacion { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        public string estadoContribuyente { get; set; }
        public string claseContribuyente { get; set; }
        public string fechaInicioActividades { get; set; }
        public string fechaReinicioActividades { get; set; }
        public string tipoContribuyente { get; set; }
        public string nombreFantasiaComercial { get; set; }
        public string actividadEconomica { get; set; }
        public rsDireccionRUC direccion { get; set; }
        public string codigoCIUU { get; set; }
        public string codigoCIUUSB { get; set; }
        public string idCodigoNeoCIUU { get; set; }

        public rsInfoRUCPersona()
        {
            direccion = new rsDireccionRUC();
        }
    }

    public class rsDireccionRUC
    {
        public string calle { get; set; }
        public string numero { get; set; }
        public string interseccion { get; set; }

    }
}