using bg.hd.banca.persona.domain.entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class RetomarSolicitudResponse
    {
        string _codigoRetorno = string.Empty;
        string _mensaje = string.Empty;
        bool _tieneSolicitudesEnProceso = false;
        string _idExpediente = string.Empty;
        string _idSolicitud = string.Empty;
        string _usuario = string.Empty;
        string _opidUsuario = string.Empty;
        string _autenticacion = string.Empty;
        string _redireccionar = string.Empty;        
        bool _tieneSolicitudesEnOficina = false;
        string _montoSolicitado = string.Empty;
        string _diasVigencia = string.Empty;
        string _guid_persona = string.Empty;
        public string nombreProducto { get ; set ; }
        public string idProducto { get; set; }
        Transaccion dataTransaccion = new Transaccion();

        public string codigoRetorno { get => _codigoRetorno; set => _codigoRetorno = value; }
        public string mensaje { get => _mensaje; set => _mensaje = value; }
        public bool tieneSolicitudesEnProceso { get => _tieneSolicitudesEnProceso; set => _tieneSolicitudesEnProceso = value; }
        public string idExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string idSolicitud { get => _idSolicitud; set => _idSolicitud = value; }
        public string usuario { get => _usuario; set => _usuario = value; }
        public string opidUsuario { get => _opidUsuario; set => _opidUsuario = value; }
        public string autenticacion { get => _autenticacion; set => _autenticacion = value; }
        public string redireccionar { get => _redireccionar; set => _redireccionar = value; }
        public string montoSolicitado { get => _montoSolicitado; set => _montoSolicitado = value; }
        public string diasVigencia { get => _diasVigencia; set => _diasVigencia = value; }
        public string guid_persona { get => _guid_persona; set => _guid_persona = value; }
        public bool tieneSolicitudesEnOficina { get => _tieneSolicitudesEnOficina; set => _tieneSolicitudesEnOficina = value; }        
        [JsonIgnoreAttribute] public Transaccion DataTransaccion { get => dataTransaccion; set => dataTransaccion = value; }
    }
}
