using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ActualizarExpedienteRequest
    {
        string _identificacion = string.Empty;
        string _idExpediente = string.Empty;
        string _idPersona = string.Empty;
        string _idProducto = string.Empty;
        string _usuarioEtapa = string.Empty;
        string _nombreEnTarjeta = string.Empty;
        string _nombreEnLibretaChequera = string.Empty;
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
        [JsonPropertyName("idTipoCuentaCredito")]
        public string? Ctctipo { get; set; } = "";
        [JsonPropertyName("idNumCuentaCredito")]
        public string? CtcNumero { get; set; } = "";
        [JsonPropertyName("idNumCuentaDebito")]
        public string? NumCuentaDebito { get; set; } = "";
        [JsonPropertyName("idTipoCuentaDebito")]
        public string? TipoCuentaDebito { get; set; } = "";
        [JsonPropertyName("isESigned")]
        public bool? tieneFirmaElectronica { get; set; } = false;
        public string UsuarioGestor { get; set; } = "";
        public string OpidGestor { get; set; } = "";
        public string idAgenciaOficial { get; set; }

        string _NumCuentaCredito = string.Empty;
        string _NumCuentaDebito = string.Empty;
        string _idDireccion = string.Empty;
        string _requiereBancontrol = string.Empty;
        string _idModulo = string.Empty;
        string _idFormulario = string.Empty;
        string _strUsuario = string.Empty;
        string _idSolicitud = string.Empty;
        string _cuota = string.Empty;
        string _tasaInteresSolicada = string.Empty;
        string _tieneCuenta = string.Empty;
        /***/
        /// <summary>
        /// identificacion
        /// </summary>
        /// <example>1600288409</example>
        [Required] public string identificacion { get => _identificacion; set => _identificacion = value; }
        /// <summary>
        /// idExpediente
        /// </summary>
        /// <example>10956018</example>
        [Required] public string idExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string? idPersona { get => _idPersona; set => _idPersona = value; }
        /// <summary>
        /// idProducto
        /// </summary>
        /// <example>cuotaMensual</example>
        [Required] public string idProducto { get => _idProducto; set => _idProducto = value; }
        /// <summary>
        /// usuarioEtapa
        /// </summary>
        /// <example>null</example>
        public string? usuarioEtapa { get => _usuarioEtapa; set => _usuarioEtapa = value; }
        /// <summary>
        /// nombreEnTarjeta
        /// </summary>
        /// <example>null</example>
        public string? nombreEnTarjeta { get => _nombreEnTarjeta; set => _nombreEnTarjeta = value; }
        /// <summary>
        /// nombreEnLibretaChequera
        /// </summary>
        /// <example>null</example>
        public string? nombreEnLibretaChequera { get => _nombreEnLibretaChequera; set => _nombreEnLibretaChequera = value; }
        /// <summary>
        /// montoFinanciar
        /// </summary>
        /// <example>12345</example>
        public string? montoFinanciar { get => _montoFinanciar; set => _montoFinanciar = value; }
        /// <summary>
        /// tasaInteresProducto
        /// </summary>
        /// <example>10.72</example>
        public string? tasaInteresProducto { get => _tasaInteresProducto; set => _tasaInteresProducto = value; }
        /// <summary>
        /// idPeriodicidad
        /// </summary>
        /// <example>null</example>
        public string? idPeriodicidad { get => _idPeriodicidad; set => _idPeriodicidad = value; }
        /// <summary>
        /// plazo
        /// </summary>
        /// <example>24</example>
        public string? plazo { get => _plazo; set => _plazo = value; }
        /// <summary>
        /// diaPago
        /// </summary>
        /// <example>fija</example>
        public string? diaPago { get => _diaPago; set => _diaPago = value; }
        /// <summary>
        /// idTipoAmortizacion
        /// </summary>
        /// <example>2</example>
        public string? idTipoAmortizacion { get => _idTipoAmortizacion; set => _idTipoAmortizacion = value; }
        /// <summary>
        /// idGarantiaCredito
        /// </summary>
        /// <example>null</example>
        public string? idGarantiaCredito { get => _idGarantiaCredito; set => _idGarantiaCredito = value; }
        /// <summary>
        /// idSubtipoGarantia
        /// </summary>
        /// <example>null</example>
        public string? idSubtipoGarantia { get => _idSubtipoGarantia; set => _idSubtipoGarantia = value; }
        /// <summary>
        /// idCompaniaAseguradora
        /// </summary>
        /// <example>null</example>
        public string? idCompaniaAseguradora { get => _idCompaniaAseguradora; set => _idCompaniaAseguradora = value; }
        /// <summary>
        /// propositoCredito
        /// </summary>
        /// <example>null</example>
        public string? propositoCredito { get => _propositoCredito; set => _propositoCredito = value; }
        /// <summary>
        /// idPaisDestinoFondos
        /// </summary>
        /// <example>null</example>
        public string? idPaisDestinoFondos { get => _idPaisDestinoFondos; set => _idPaisDestinoFondos = value; }
        /// <summary>
        /// idProvinciaDestinoFondos
        /// </summary>
        /// <example>null</example>
        public string? idProvinciaDestinoFondos { get => _idProvinciaDestinoFondos; set => _idProvinciaDestinoFondos = value; }
        /// <summary>
        /// idCiudadDestinoFondos
        /// </summary>
        /// <example>null</example>
        public string? idCiudadDestinoFondos { get => _idCiudadDestinoFondos; set => _idCiudadDestinoFondos = value; }
        /// <summary>
        /// idParroquiaDestinoFondos
        /// </summary>
        /// <example>null</example>
        public string? idParroquiaDestinoFondos { get => _idParroquiaDestinoFondos; set => _idParroquiaDestinoFondos = value; }
        /// <summary>
        /// bancoCtaCreditoDebito
        /// </summary>
        /// <example>null</example>
        public string? bancoCtaCreditoDebito { get => _bancoCtaCreditoDebito; set => _bancoCtaCreditoDebito = value; }
        /// <summary>
        /// idDireccion
        /// </summary>
        /// <example>null</example>
        public string? idDireccion { get => _idDireccion; set => _idDireccion = value; }
        /// <summary>
        /// requiereBancontrol
        /// </summary>
        /// <example>null</example>
        public string? requiereBancontrol { get => _requiereBancontrol; set => _requiereBancontrol = value; }
        /// <summary>
        /// idModulo
        /// </summary>
        /// <example>null</example>
        public string? idModulo { get => _idModulo; set => _idModulo = value; }
        /// <summary>
        /// idFormulario
        /// </summary>
        /// <example>null</example>
        public string? idFormulario { get => _idFormulario; set => _idFormulario = value; }
        /// <summary>
        /// strUsuario
        /// </summary>
        /// <example>null</example>
        public string strUsuario { get => _strUsuario; set => _strUsuario = value; }
        /// <summary>
        /// idSolicitud
        /// </summary>
        /// <example>229148</example>
        public string? idSolicitud { get => _idSolicitud; set => _idSolicitud = value; }
        /// <summary>
        /// cuota
        /// </summary>
        /// <example>null</example>
        public string? cuota { get => _cuota; set => _cuota = value; }
        /// <summary>
        /// tasaInteresSolicada
        /// </summary>
        /// <example>null</example>
        public string? tasaInteresSolicada { get => _tasaInteresSolicada; set => _tasaInteresSolicada = value; }
        /// <summary>
        /// tieneCuenta
        /// </summary>
        /// <example>null</example>
        public string? tieneCuenta { get => _tieneCuenta; set => _tieneCuenta = value; }
        /***/
    }
}