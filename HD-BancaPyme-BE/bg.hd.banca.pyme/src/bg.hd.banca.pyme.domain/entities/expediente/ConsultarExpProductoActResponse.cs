using bg.hd.banca.pyme.domain.entities.config;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultarExpProductoActResponse
    {
        public int codigoRetorno { get; set; } = 0;
        public string mensaje { get; set; } = "";
        public InfoExpediente infoExpediente { get; set; } = new InfoExpediente();
        public InfoProductoActivo infoProductoActivo { get; set; } = new InfoProductoActivo();
        public Transaccion DataTransaccion { get; set; } = new Transaccion();

        public class InfoExpediente
        {
            public string idExpediente { get; set; } = "";
            public string subProductoCore { get; set; } = "";
            public string tipoIdentificacion { get; set; } = "";
            public string observacionAnalistaCredito { get; set; } = "";
            public string numSolicitudHost { get; set; } = "";
            public string usuarioGestor { get; set; } = "";
            public string idEtapa { get; set; } = "";
            public string etapaDescripcion { get; set; } = "";

        }
        public class InfoProductoActivo
        {
            public int codigoRetorno { get; set; } = 0;
            public string mensaje { get; set; } = "";
            public string? nombreLibretaChequera         { get; set; } = "";
            public string? montoFinanciar                { get; set; } = "";
            public string? montoSugeridoAnalistaCredito  { get; set; } = "";
            public string? tasaInteresProducto           { get; set; } = "";
            public string? plazo                         { get; set; } = "";
            public string? diaPago                       { get; set; } = "";
            public string? valorDividendo                { get; set; } = "";
            public string? idTablaAmortizacion           { get; set; } = "";
            public string? idTablaAmortizacionDesc       { get; set; } = "";
            public string? idNumCuentaDebito             { get; set; } = "";
            public string? idTipoCuentaDebito            { get; set; } = "";
            public string? idTipoCuentaDebitoDesc        { get; set; } = "";
            public string? idTipoCuentaCredito           { get; set; } = "";
            public string? idTipoCuentaCreditoDesc       { get; set; } = "";
            public string? idNumCuentaCredito            { get; set; } = "";
            public string identificacion                 { get; set; } = "";
            public string ingresoMensual                 { get; set; } = "";
            public string plazoSugeridoAnalistaCredito   { get; set; } = "";

        }

    }
}
