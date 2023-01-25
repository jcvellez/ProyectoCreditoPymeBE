using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ActualizarExpedienteMicroRequest
    {
        string _identificacion = string.Empty;
        string _idExpediente = string.Empty;
        string _idPersona = string.Empty;
        string _idProducto = string.Empty;
        string _usuarioEtapa = string.Empty;
        string _nombreEnTarjeta = string.Empty;
        string _nombreLibretaChequera = string.Empty;
        string _montoFinanciar = string.Empty;
        string _tasaInteresProducto = string.Empty;
        string _idPeriodicidad = string.Empty;
        string _plazo = string.Empty;
        string _diaPago = string.Empty;
        string _idTipoAmortizacion = string.Empty;
        string _idGarantiaCredito = string.Empty;
        string _idSubtipoGarantia = string.Empty;
        string _idCompaniaAseguradora = string.Empty;
        string _propositoCredito = string.Empty;
        string _idPaisDestinoFondos = string.Empty;
        string _idProvinciaDestinoFondos = string.Empty;
        string _idCiudadDestinoFondos = string.Empty;
        string _idParroquiaDestinoFondos = string.Empty;
        string _bancoCtaCreditoDebito = string.Empty;
        string _idTipoCuentaCredito = string.Empty;
        string _idNumCuentaCredito = string.Empty;
        string _idTipoCuentaDebito = string.Empty;
        string _idNumCuentaDebito = string.Empty;
        string _idDireccion = string.Empty;
        string _requiereBancontrol = string.Empty;
        string _idModulo = string.Empty;
        string _idFormulario = string.Empty;
        string _strUsuario = string.Empty;
        string _idSolicitud = string.Empty;
        string _cuota = string.Empty;
        string _tasaInteresSolicada = string.Empty;
        public string? idBancoCredito { get; set; } = "";
        public string? idBancoDebito { get; set; } = "";
        /***/
        [Required] public string identificacion { get => _identificacion; set => _identificacion = value; }
        [Required] public string idExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string? idPersona { get => _idPersona; set => _idPersona = value; }
        [Required] public string idProducto { get => _idProducto; set => _idProducto = value; }
        public string? usuarioEtapa { get => _usuarioEtapa; set => _usuarioEtapa = value; }
        public string? nombreEnTarjeta { get => _nombreEnTarjeta; set => _nombreEnTarjeta = value; }
        public string? nombreLibretaChequera { get => _nombreLibretaChequera; set => _nombreLibretaChequera = value; }
        public string? montoFinanciar { get => _montoFinanciar; set => _montoFinanciar = value; }
        public string? tasaInteresProducto { get => _tasaInteresProducto; set => _tasaInteresProducto = value; }
        public string? idPeriodicidad { get => _idPeriodicidad; set => _idPeriodicidad = value; }
        public string? plazo { get => _plazo; set => _plazo = value; }
        public string? diaPago { get => _diaPago; set => _diaPago = value; }
        public string? idTipoAmortizacion { get => _idTipoAmortizacion; set => _idTipoAmortizacion = value; }
        public string? idGarantiaCredito { get => _idGarantiaCredito; set => _idGarantiaCredito = value; }
        public string? idSubtipoGarantia { get => _idSubtipoGarantia; set => _idSubtipoGarantia = value; }
        public string? idCompaniaAseguradora { get => _idCompaniaAseguradora; set => _idCompaniaAseguradora = value; }
        public string? propositoCredito { get => _propositoCredito; set => _propositoCredito = value; }
        public string? idPaisDestinoFondos { get => _idPaisDestinoFondos; set => _idPaisDestinoFondos = value; }
        public string? idProvinciaDestinoFondos { get => _idProvinciaDestinoFondos; set => _idProvinciaDestinoFondos = value; }
        public string? idCiudadDestinoFondos { get => _idCiudadDestinoFondos; set => _idCiudadDestinoFondos = value; }
        public string? idParroquiaDestinoFondos { get => _idParroquiaDestinoFondos; set => _idParroquiaDestinoFondos = value; }
        public string? bancoCtaCreditoDebito { get => _bancoCtaCreditoDebito; set => _bancoCtaCreditoDebito = value; }
        [JsonIgnore] public string? idTipoCuentaCredito { get => _idTipoCuentaCredito; set => _idTipoCuentaCredito = value; }
        [JsonIgnore] public string? idNumCuentaCredito { get => _idNumCuentaCredito; set => _idNumCuentaCredito = value; }
        [JsonIgnore] public string? idTipoCuentaDebito { get => _idTipoCuentaDebito; set => _idTipoCuentaDebito = value; }
        [JsonIgnore] public string? idNumCuentaDebito { get => _idNumCuentaDebito; set => _idNumCuentaDebito = value; }
        public string? idDireccion { get => _idDireccion; set => _idDireccion = value; }
        public string? requiereBancontrol { get => _requiereBancontrol; set => _requiereBancontrol = value; }
        public string? idModulo { get => _idModulo; set => _idModulo = value; }
        public string? idFormulario { get => _idFormulario; set => _idFormulario = value; }
        public string strUsuario { get => _strUsuario; set => _strUsuario = value; }
        public string? idSolicitud { get => _idSolicitud; set => _idSolicitud = value; }
        public string? cuota { get => _cuota; set => _cuota = value; }
        public string? tasaInteresSolicada { get => _tasaInteresSolicada; set => _tasaInteresSolicada = value; }

        /***/
    }
}