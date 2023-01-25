using bg.hd.banca.persona.domain.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class VerificaSolicitudesResponseMicro
    {
        string _codigoError = "0";
        public string? mensajeError { get; set; }
        List<SolicitudesProductoContratado> solicitudesProductoContratado = new List<SolicitudesProductoContratado>();
        ContratarProducto contratarProducto = new ContratarProducto();
        List<SolicitudesProcesoContratacion> solicitudesProcesoContratacion = new List<SolicitudesProcesoContratacion>();
        Transaccion dataTransaccion = new Transaccion();
        Producto dtosProducto = new Producto();

        public string codigoError { get => _codigoError; set => _codigoError = value; }
        public ContratarProducto ContratarProducto { get => contratarProducto; set => contratarProducto = value; }

        public List<SolicitudesProductoContratado> SolicitudesProductoContratado { get => solicitudesProductoContratado; set => solicitudesProductoContratado = value; }

        public List<SolicitudesProcesoContratacion> SolicitudesProcesoContratacion { get => solicitudesProcesoContratacion; set => solicitudesProcesoContratacion = value; }
        public Transaccion DataTransaccion { get => dataTransaccion; set => dataTransaccion = value; }
        public Producto DtosProducto { get => dtosProducto; set => dtosProducto = value; }
    }

    public class SolicitudesProductoContratado
    {

        string _idExpediente = string.Empty;
        string _idProducto = string.Empty;
        string _idProductoPadre = string.Empty;
        string _numIdentificacion = string.Empty;
        string _nombreCompleto = string.Empty;
        string _fechaExpediente = string.Empty;
        string _idEtapa = string.Empty;
        string _etapaExpediente = string.Empty;
        string _idEstado = string.Empty;
        string _estadoExpediente = string.Empty;

        public string idExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string idProducto { get => _idProducto; set => _idProducto = value; }
        public string idProductoPadre { get => _idProductoPadre; set => _idProductoPadre = value; }
        public string numIdentificacion { get => _numIdentificacion; set => _numIdentificacion = value; }
        public string nombreCompleto { get => _nombreCompleto; set => _nombreCompleto = value; }
        public string fechaExpediente { get => _fechaExpediente; set => _fechaExpediente = value; }
        public string idEtapa { get => _idEtapa; set => _idEtapa = value; }
        public string EtapaExpediente { get => _etapaExpediente; set => _etapaExpediente = value; }
        public string idEstado { get => _idEstado; set => _idEstado = value; }
        public string estadoExpediente { get => _estadoExpediente; set => _estadoExpediente = value; }
    }

    public class ContratarProducto
    {
        string _puedeContratarProducto = string.Empty;
        public string puedeContratarProducto { get => _puedeContratarProducto; set => _puedeContratarProducto = value; }
    }

    public class SolicitudesProcesoContratacion
    {

        string _idExpediente = string.Empty;
        string _idProducto = string.Empty;
        string _idProductoPadre = string.Empty;
        string _idTipoIdentificacion = string.Empty;
        string _numIdentificacion = string.Empty;
        string _nombreCompleto = string.Empty;
        string _fechaExpediente = string.Empty;
        string _idEtapa = string.Empty;
        string _EtapaExpediente = string.Empty;
        string _idEstado = string.Empty;
        string _EstadoExpediente = string.Empty;
        string _usuarioGestor = string.Empty;
        string _opidGestor = string.Empty;
        string _idSolicitud = string.Empty;
        string _montoCupoSolicitado = string.Empty;
        string _numSolicitudHost = string.Empty;
        string _idActividadCIIU = string.Empty;
        string _idCanal = string.Empty;
        string _pantalla = string.Empty;

        public string IdExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string IdProducto { get => _idProducto; set => _idProducto = value; }
        public string IdProductoPadre { get => _idProductoPadre; set => _idProductoPadre = value; }
        public string IdTipoIdentificacion { get => _idTipoIdentificacion; set => _idTipoIdentificacion = value; }
        public string NumIdentificacion { get => _numIdentificacion; set => _numIdentificacion = value; }
        public string NombreCompleto { get => _nombreCompleto; set => _nombreCompleto = value; }
        public string FechaExpediente { get => _fechaExpediente; set => _fechaExpediente = value; }
        public string IdEtapa { get => _idEtapa; set => _idEtapa = value; }
        public string EtapaExpediente { get => _EtapaExpediente; set => _EtapaExpediente = value; }
        public string IdEstado { get => _idEstado; set => _idEstado = value; }
        public string EstadoExpediente { get => _EstadoExpediente; set => _EstadoExpediente = value; }
        public string UsuarioGestor { get => _usuarioGestor; set => _usuarioGestor = value; }
        public string OpidGestor { get => _opidGestor; set => _opidGestor = value; }
        public string IdSolicitud { get => _idSolicitud; set => _idSolicitud = value; }
        public string MontoCupoSolicitado { get => _montoCupoSolicitado; set => _montoCupoSolicitado = value; }
        public string NumSolicitudHost { get => _numSolicitudHost; set => _numSolicitudHost = value; }
        public string IdActividadCIIU { get => _idActividadCIIU; set => _idActividadCIIU = value; }
        public string IdCanal { get => _idCanal; set => _idCanal = value; }
        public string Pantalla { get => _pantalla; set => _pantalla = value; }
    }

}
