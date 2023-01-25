using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class CuentasContablesRequest
    {
        /// <summary>
        /// idProceso del cliente
        /// </summary>
        /// <example>43486</example>
        public int? idProceso { get; set; }
        /// <summary>
        /// idClienteRating del cliente
        /// </summary>
        /// <example>31319</example>
        public string? idClienteRating { get; set; }
        /// <summary>
        /// identificacion del cliente
        /// </summary>
        /// <example>0926084039</example>
        public string? identificacion { get; set; }
        [JsonIgnore]
        public string? tipoIdentificacion { get; set; }
        [JsonIgnore]
        public string? usuario { get; set; }
        /// <summary>
        /// fechaRevision
        /// </summary>
        /// <example>19-07-2022</example>
        public string? fechaRevision { get; set; }
        /// <summary>
        /// caja
        /// </summary>
        /// <example>100000.53</example>
        public string? act_caja { get; set; }
        /// <summary>
        /// cuentasCobrar
        /// </summary>
        /// <example>500000.00</example>
        public string? act_cuentasCobrar { get; set; }
        /// <summary>
        /// totalActfijos
        /// </summary>
        /// <example>40000.00</example>
        public string? act_totalActfijos { get; set; }
        /// <summary>
        /// totalInventario
        /// </summary>
        /// <example>220000.00</example>
        public string? act_totalInventario { get; set; }
        /// <summary>
        /// obligacionesBancarias corto plazo
        /// </summary>
        /// <example>34000.00</example>        
        public string? pas_obligacionBancariaCP { get; set; }
        /// <summary>
        /// pasivosNoCorrientes
        /// </summary>
        /// <example>300000.00</example>
        public string? pas_pasivosNoCtes { get; set; }   
        /// <summary>
        /// otrosPasivos
        /// </summary>
        /// <example>47000.00</example>
        public string? pas_otrosPasivos { get; set; }
        /// <summary>
        /// obligacionesBancariaslargoplazo
        /// </summary>
        /// <example>48000.00</example>
        public string? pas_obligacionBancariaLP { get; set; }

        /// <example>cuotaMensual</example>
        public string? Producto { get; set; }
        /// <example>72323</example>
        public string? IdSolicitud { get; set; }
        [JsonIgnore]
        public List<CuentasContables>? cuentasContables { get; set; }
    
        public class CuentasContables
        {
            string _idCuenta = string.Empty;
            string _saldo = string.Empty;
            string _tipo = string.Empty;

            public string idCuenta { get => _idCuenta; set => _idCuenta = value; }
            public string saldo { get => _saldo; set => _saldo = value; }
            public string tipo { get => _tipo; set => _tipo = value; }

        }
    }    
}
