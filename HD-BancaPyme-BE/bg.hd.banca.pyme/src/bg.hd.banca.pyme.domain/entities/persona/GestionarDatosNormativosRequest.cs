using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GestionarDatosNormativosRequest
    {
		/// <summary>
		/// Producto 
		/// </summary>
		/// <example>cuotaMensual</example>
		[Required] public string producto { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>0917975294</example>
		public string identificacionCliente { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>9817988</example>
		public string idExpediente { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>neoweb</example>		
		public string usuario { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>S</example>
		public string preguntaPaisNacimientoDiferenteEcuador { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>2183</example>
		public string? paisNacimiento { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>S</example>
		[JsonIgnore]
		public string preguntaTienesRucActivo { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>0917975294001</example>
		[JsonIgnore]
		public string numeroRUC { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>S</example>
		[Required] public string? tieneResidenciaFiscalExterior { get; set; } = "S";
		public List<ResidenciaFiscal> residenciaFiscalExterior { get; set; } = new List<ResidenciaFiscal>();
	}
}
