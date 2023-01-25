using bg.hd.banca.pyme.domain.entities.NewtonsoftJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaResidenciaFiscalResponse
    {
		public int CodigoRetorno { get; set; } = 1;
		public string Mensaje { get; set; } = "";
		[JsonProperty("listadoResidenciaFiscal")]
		[JsonConverter(typeof(SingleValueArrayConverter<ResidenciaFiscalSoapResponse>))]
		public List<ResidenciaFiscalSoapResponse> listadoResidenciaFiscal { get; set; } = new List<ResidenciaFiscalSoapResponse>();
	}
	public class ResidenciaFiscalSoapResponse
	{
		public int codigoR { get; set; } = 0;
		public string? codigoPaisR { get; set; } = "0";
		public string? nifR { get; set; } = "";
		public string? estado { get; set; } = "";
		public string? paisR { get; set; } = "";
		public string? DireccionR { get; set; } = "";
	}
}
