using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GestionResidenciaFiscalRequest
    {
		[Required] public string producto { get; set; } = "";
		public bool controlarExcepcion { get; set; } = true;
		/// <summary>
		/// </summary>
		/// <example>1</example>
		public string? opcion { get; set; } = "1";
		/// <summary>
		/// </summary>
		/// <example>525</example>
		[Required] public string? idTipoIdentificacion { get; set; } = "0";
		/// <summary>
		/// </summary>
		/// <example>0917975294</example>
		[Required] public string? numIdentificacion { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>neoWeb</example>
		public string? usuario { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>S</example>
		[Required] public string? tieneResidenciaFiscalExterior { get; set; } = "S";
		public ResidenciaFiscalExterior? ResidenciaFiscalExterior { get; set; } = new ResidenciaFiscalExterior();
	}

	public class ResidenciaFiscalExterior
	{
		public List<ResidenciaFiscal> residenciaFiscal { get; set; } = new List<ResidenciaFiscal>();

	}

	public class ResidenciaFiscal
	{
		/// <summary>
		/// </summary>
		/// <example>2179</example>
		public string? codigoPaisR { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>456464654</example>
		public string? nifR { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>1</example>
		public string? estado { get; set; } = "1";
		/// <summary>
		/// </summary>
		/// <example>-1</example>
		public int codigoR { get; set; } = -1;
		/// <summary>
		/// </summary>
		/// <example>11 Y La D Pasando El Puente De La A</example>
		public string? DireccionR { get; set; } = "";

	}
}
