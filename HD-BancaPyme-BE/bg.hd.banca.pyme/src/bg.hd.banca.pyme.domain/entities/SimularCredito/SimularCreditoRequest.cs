using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.SimularCredito
{
    public class SimularCreditoRequest
    {
        /// <summary>
        /// cedula 
        /// </summary>
        /// <example>0703691824</example>
        public string? cedula { get; set; }
        /// <summary>
        /// idSolicitud 
        /// </summary>
        /// <example>225920</example>
        public string? idSolicitud { get; set; }
        //valida si consulta datos de BD o toma datos enviados
        //si envia R->se calcula con datos DB si envia S simula nuevos datos
        /// <summary>
        /// tipoCalculo 
        /// </summary>
        /// <example>s</example>
        public string? tipoCalculo { get; set; }
        /// <summary>
        /// monto 
        /// </summary>
        /// <example>40000</example>
        public int? monto { get; set; }
        /// <summary>
        /// destinoFinanciero 
        /// </summary>
        /// <example>capitalTrabajo</example>
        public string? destinoFinanciero { get; set; }
        /// <summary>
        /// tipoProducto 
        /// </summary>
        /// <example>cuotaMensual</example>
        public string? tipoProducto { get; set; }
        /// <summary>
        /// plazo 
        /// </summary>
        /// <example>30</example>
        public int? plazo { get; set; }
        /// <summary>
        /// tipoCuota 
        /// </summary>
        /// <example>fija</example>
        public string? tipoCuota { get; set; }
        /// <summary>
        /// diaPago 
        /// </summary>
        /// <example>1</example>
        public int? diaPago { get; set; }
    }
}
