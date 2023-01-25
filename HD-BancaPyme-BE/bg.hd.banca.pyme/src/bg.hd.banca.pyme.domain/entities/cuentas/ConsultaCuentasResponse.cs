using bg.hd.banca.pyme.domain.entities.NewtonsoftJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.cuentas
{
    public class ConsultaCuentasResponse
    {
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = "";
        public INFORMACION INFORMACION = new INFORMACION();
    }
    public class INFORMACION
    {
        [JsonProperty("PRODUCTOS")]
        [JsonConverter(typeof(SingleValueArrayConverter<CUENTAS_OUT>))]
        public List<CUENTAS_OUT> CUENTAS_OUT { get; set; }

        public INFORMACION()
        {
            this.CUENTAS_OUT = new List<CUENTAS_OUT>();
        }

    }
    public class CUENTAS_OUT
    {

        //[JsonProperty("IDCONTROLPRODUCTOS")] 
        //public string IDCONTROLCUENTAS_OUT { get; set; } = "";
        [JsonProperty("OPRODUCTO")]
        public string TIPO_CUENTA { get; set; } = "";//
        [JsonProperty("OCODPRODUCTO")]
        public string NUM_CUENTA { get; set; } = "";//
        [JsonProperty("OESTADO")]
        public string ESTADO_CUENTA { get; set; } = "";
        //[JsonProperty("OFECAPERTURA")]
        //public string FECHA_APERTURA { get; set; } = "";
        [JsonProperty("ORELACION")]
        public string RELACION_CLIENTE { get; set; } = "";
        //[JsonProperty("OEXPEDIENTE")]
        //public string EXPEDIENTE { get; set; } = "";
        //[JsonProperty("OSOLICITUD")]
        //public string SOLICITUD { get; set; } = "";
        //[JsonProperty("OFECHAACTUALIZASOLI")]
        //public string FECHA_ACTUALIZA_SOLI { get; set; } = "";
        //[JsonProperty("OESTADOSOLICITUD")]
        //public string ESTADOSOLICITUD { get; set; } = "";
        //[JsonProperty("OPIDSOLICITUDCTA")]
        //public string OPIDSOLICITUDCTA { get; set; } = "";
        //[JsonProperty("SALDOEFECTIVOCTA")]
        //public string SALDOEFECTIVOCTA { get; set; } = "";
        //[JsonProperty("SIGNO-SALDO")]
        //public string SIGNO_SALDO { get; set; } = "";
    }
}
