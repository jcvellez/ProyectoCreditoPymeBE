using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GestionValidarPoliticasRequest
	{
		/// <summary>
		/// Producto 
		/// </summary>
		/// <example>cuotaMensual</example>
		[Required] public string producto { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>0917975294</example>
		public string identificacion { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>9817988</example>
		public string idExpediente { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>neoweb</example>
		[JsonIgnore]
		public string usuario { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>24</example>
		public string PlazoSolicitado { get; set; } = "";
		/// <summary>
		/// </summary>
		/// <example>15000</example>
		public string MontoSolicitado { get; set; } = "";
		public string UsuarioGestor { get; set; } = "";
		public string OpidGestor { get; set; } = "";
		public string idAgenciaOficial { get; set; }

	}
}
