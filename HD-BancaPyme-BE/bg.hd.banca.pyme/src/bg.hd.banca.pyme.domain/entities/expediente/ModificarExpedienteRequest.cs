using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ModificarExpedienteRequest
    {
        string identificacion = string.Empty;
        string _idExpediente = string.Empty;
        string _idProducto = string.Empty;
        string _idCatDictamen = string.Empty;
        string _idDictamen = string.Empty;
        string _motivoDictamen = string.Empty;
        string _usuarioModifica = string.Empty;
        string _usuarioGestor = string.Empty;
        string _opidGestor = string.Empty;
        string _idCatEstado = string.Empty;
        string _idEstado = string.Empty;
        string _idCatEtapa = string.Empty;
        string _idEtapa = string.Empty;
        string _usuarioEtapa = string.Empty;
        string _idCatEstadoEtapa = string.Empty;
        string _idEstadoEtapa = string.Empty;
        string _numeroCuenta = string.Empty;
        string _numeroPrestamo = string.Empty;
        string _numSolicitudHost = string.Empty;
        string _fechaSolicitudHost = string.Empty;
        string _fechaLiquidaSolicitudHost = string.Empty;
        string _estadoSolicitudHost = string.Empty;
        string _numOrdenOperacionHost = string.Empty;
        string _fechaOrdenOperacionHost = string.Empty;
        string _estadoOrdenOperacionHost = string.Empty;
        string _opidSolicitudHost = string.Empty;
        string _usuarioSolicitudHost = string.Empty;
        string _etapaExpMicroAplicativo = string.Empty;
        string _estadoExpMicroAplicativo = string.Empty;
        string _direccionEnZonaVetada = string.Empty;
        string _requiereVerificacionDomicilio = string.Empty;
        string _marcaAnalisisFC = string.Empty;
        string _observaciones = string.Empty;
        string _idModulo = string.Empty;
        string _idFormulario = string.Empty;
        string _strUsuario = string.Empty;
        string _idCatMotivoRechazo = string.Empty;
        string _idMotivoRechazo = string.Empty;
        string _comentarioRechazo = string.Empty;
        string _numSolSyscard = string.Empty;
        string _numAliasTarjetasCredito = string.Empty;
        string _usuarioSolTarjetaCredito = string.Empty;
        string _estadoSolTarjetaCredito = string.Empty;
        string _fechaSolTarjetaCredito = string.Empty;
        string _numTarjetaBancontrol = string.Empty;
        string _realizoVerificacion = string.Empty;
        string _requiereConfirmacionDatos = string.Empty;
        string _idDictamenComercial = string.Empty;
        string _numCotizacion = string.Empty;
        string _idExpedienteRelacionado = string.Empty;
        string _numOrdenRetanqueo = string.Empty;
        string _tipoOperacionRetanqueo = string.Empty;
        string _aplicaFirmaElect = string.Empty;
        string _dictamenConcesionario = string.Empty;

        public string idExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string idModulo { get => _idModulo; set => _idModulo = value; }
        public string idFormulario { get => _idFormulario; set => _idFormulario = value; }
        public string strUsuario { get => _strUsuario; set => _strUsuario = value; }
        public string idProducto { get => _idProducto; set => _idProducto = value; }
        public string idCatDictamen { get => _idCatDictamen; set => _idCatDictamen = value; }
        public string idDictamen { get => _idDictamen; set => _idDictamen = value; }
        public string motivoDictamen { get => _motivoDictamen; set => _motivoDictamen = value; }
        public string usuarioModifica { get => _usuarioModifica; set => _usuarioModifica = value; }
        public string usuarioGestor { get => _usuarioGestor; set => _usuarioGestor = value; }
        public string opidGestor { get => _opidGestor; set => _opidGestor = value; }
        public string idCatEstado { get => _idCatEstado; set => _idCatEstado = value; }
        public string idEstado { get => _idEstado; set => _idEstado = value; }
        public string idCatEtapa { get => _idCatEtapa; set => _idCatEtapa = value; }
        public string idEtapa { get => _idEtapa; set => _idEtapa = value; }
        public string usuarioEtapa { get => _usuarioEtapa; set => _usuarioEtapa = value; }
        public string idCatEstadoEtapa { get => _idCatEstadoEtapa; set => _idCatEstadoEtapa = value; }
        public string idEstadoEtapa { get => _idEstadoEtapa; set => _idEstadoEtapa = value; }
        public string numeroCuenta { get => _numeroCuenta; set => _numeroCuenta = value; }
        public string numeroPrestamo { get => _numeroPrestamo; set => _numeroPrestamo = value; }
        public string numSolicitudHost { get => _numSolicitudHost; set => _numSolicitudHost = value; }
        public string fechaSolicitudHost { get => _fechaSolicitudHost; set => _fechaSolicitudHost = value; }
        public string fechaLiquidaSolicitudHost { get => _fechaLiquidaSolicitudHost; set => _fechaLiquidaSolicitudHost = value; }
        public string estadoSolicitudHost { get => _estadoSolicitudHost; set => _estadoSolicitudHost = value; }
        public string numOrdenOperacionHost { get => _numOrdenOperacionHost; set => _numOrdenOperacionHost = value; }
        public string fechaOrdenOperacionHost { get => _fechaOrdenOperacionHost; set => _fechaOrdenOperacionHost = value; }
        public string estadoOrdenOperacionHost { get => _estadoOrdenOperacionHost; set => _estadoOrdenOperacionHost = value; }
        public string opidSolicitudHost { get => _opidSolicitudHost; set => _opidSolicitudHost = value; }
        public string usuarioSolicitudHost { get => _usuarioSolicitudHost; set => _usuarioSolicitudHost = value; }
        public string etapaExpMicroAplicativo { get => _etapaExpMicroAplicativo; set => _etapaExpMicroAplicativo = value; }
        public string estadoExpMicroAplicativo { get => _estadoExpMicroAplicativo; set => _estadoExpMicroAplicativo = value; }
        public string direccionEnZonaVetada { get => _direccionEnZonaVetada; set => _direccionEnZonaVetada = value; }
        public string requiereVerificacionDomicilio { get => _requiereVerificacionDomicilio; set => _requiereVerificacionDomicilio = value; }
        public string marcaAnalisisFC { get => _marcaAnalisisFC; set => _marcaAnalisisFC = value; }
        public string observaciones { get => _observaciones; set => _observaciones = value; }
        public string idCatMotivoRechazo { get => _idCatMotivoRechazo; set => _idCatMotivoRechazo = value; }
        public string idMotivoRechazo { get => _idMotivoRechazo; set => _idMotivoRechazo = value; }
        public string comentarioRechazo { get => _comentarioRechazo; set => _comentarioRechazo = value; }
        public string numSolSyscard { get => _numSolSyscard; set => _numSolSyscard = value; }
        public string numAliasTarjetasCredito { get => _numAliasTarjetasCredito; set => _numAliasTarjetasCredito = value; }
        public string usuarioSolTarjetaCredito { get => _usuarioSolTarjetaCredito; set => _usuarioSolTarjetaCredito = value; }
        public string estadoSolTarjetaCredito { get => _estadoSolTarjetaCredito; set => _estadoSolTarjetaCredito = value; }
        public string fechaSolTarjetaCredito { get => _fechaSolTarjetaCredito; set => _fechaSolTarjetaCredito = value; }
        public string numTarjetaBancontrol { get => _numTarjetaBancontrol; set => _numTarjetaBancontrol = value; }
        public string realizoVerificacion { get => _realizoVerificacion; set => _realizoVerificacion = value; }
        public string requiereConfirmacionDatos { get => _requiereConfirmacionDatos; set => _requiereConfirmacionDatos = value; }
        public string idDictamenComercial { get => _idDictamenComercial; set => _idDictamenComercial = value; }
        public string numCotizacion { get => _numCotizacion; set => _numCotizacion = value; }
        public string idExpedienteRelacionado { get => _idExpedienteRelacionado; set => _idExpedienteRelacionado = value; }
        public string numOrdenRetanqueo { get => _numOrdenRetanqueo; set => _numOrdenRetanqueo = value; }
        public string tipoOperacionRetanqueo { get => _tipoOperacionRetanqueo; set => _tipoOperacionRetanqueo = value; }
        [JsonIgnoreAttribute] public string aplicaFirmaElect { get; set; } = "";
        public string dictamenConcesionario { get => _dictamenConcesionario; set => _dictamenConcesionario = value; }
        public string Identificacion { get => identificacion; set => identificacion = value; }
    }
}
