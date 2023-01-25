using bg.hd.banca.pyme.domain.entities.cuentas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultarCuentaPorIdResponse
    {
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = "";
        public bool crearCuenta { get; set; } = false;

        [JsonIgnore]public List<CUENTAS_OUT>? CuentasCliente { get; set; }

        public List<CUENTAS_CLIENTE>? CuentasClienteActivas { get; set; }

        public class CUENTAS_CLIENTE
        {
            public string Tipo_Cuenta { get; set; } = "";            
            public string Numero_Cuenta { get; set; } = "";
            public string Numero_Cuenta_ofuscate { get; set; } = "";
        }

        //public string idNumCuentaCredito { get; set; } = "";
        //public string idTipoCuentaCredito { get; set; } = "";
        //public string idNumCuentaDebito { get; set; } = "";
        //public string idTipoCuentaDebito { get; set; } = "";

    }
}