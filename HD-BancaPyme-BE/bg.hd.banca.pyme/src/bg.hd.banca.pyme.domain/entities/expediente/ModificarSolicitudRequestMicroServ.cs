using bg.hd.banca.pyme.domain.entities.config;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ModificarSolicitudRequestMicroServ
    {
        public string? Opcion { get; set; } = "";// 2 para actualización 
        public string? IdCanal { get; set; } = "";
        public string? IdProducto { get; set; } = "";
        public string? IdTipoIdentificacion { get; set; } = "";
        public string? Identificacion { get; set; } = "";
        public string? IdTipoPeriodicidad { get; set; } = "";
        public string? Usuario { get; set; } = "";
        public string? IdSolicitud { get; set; } = "";
        public string? MontoSolicitado { get; set; } = "";
        public string? PlazoSolicitado { get; set; } = "";
        public string? IdTipoTabla { get; set; } = "";
        public string? TasaProducto { get; set; } = "";
        public string? ValorDividendo { get; set; } = "";
        public string? ImagenReconocimientoBiometrico { get; set; } = "";
        public string? PorcentajeReconocimientoBiometrico { get; set; } = "";
        public string? RespuestaReconocimientoBiometrico { get; set; } = "";
        public string? FechaReconocimientoBiometrico { get; set; } = "";
        public string? NavegadorWeb { get; set; } = "";
        public string? DispositvoOrigen { get; set; } = "";
        public string? VersionSoOrigen { get; set; } = "";
        public string? TieneCamaraWeb { get; set; } = "";
        public string? DireccionIp { get; set; } = "";
        public string? DiaPago { get; set; } = "";
        public string? CodigoCampania { get; set; } = "";
        public string? IdTipoValidacionBiometrica { get; set; } = "";
        public string? CapacidadEndeudamientoMensual { get; set; } = "";
        public string? MaximoDeudaConsumo { get; set; } = "";
        public string? MaximaDeudaTc { get; set; } = "";
        public string? IngresoFinal { get; set; } = "";
        public string? SubProductoCore { get; set; } = "";
        public string? IdSegmentoEstrategicoCore { get; set; } = "";
        public string? PerfilCliente { get; set; } = "";
        public string? SecuenciaRiesgo { get; set; } = "";
        public string? NumIdentificacion { get; set; } = "";
        public string? Nombre { get; set; } = "";
        public string? CorreoElectronico { get; set; } = "";
        public string? IdExpediente { get; set; } = "";
        public string? AutorizaConsultaBuro { get; set; } = "";
        public string? ValidaPreguntasSeguridad { get; set; } = "";
        public string? RespuestaVelidaPreguntasSeguridad { get; set; } = "";
        public string? FechaValidaPreguntasSeguridad { get; set; } = "";
        public string? CupoSolicitado { get; set; } = "";
        public string? DigitoInicialBinTarjeta { get; set; } = "";
        public string? CorreoElectronicoCore { get; set; } = "";
        public string? TieneRegistroFirma { get; set; } = "";
        public string? NumeroSolicitudTarjetaCredito { get; set; } = "";
        public string? PaisDireccionIp { get; set; } = "";
        public string? AutenticadoBV { get; set; } = "";
        public string? NuevoCliente { get; set; } = "";
        public string? CelularCore { get; set; } = "";
        public string? MontoTotalCredito { get; set; } = "";
        public string? idEstadoActualSolicitud { get; set; } = "";
        public string? tieneActVenta { get; set; } = "";
        public string? tieneActDirNegocio { get; set; } = "";
        public string? numeroCasoActualizaVtaCRM { get; set; } = "";
        public string? numeroCasoActualizaDirNegocioCRM { get; set; } = "";
        public string? fuenteIngreso { get; set; } = "";

    }

    public class Solicitud
    {
        private ModificarSolicitudRequestMicroServ dataSolicitud = new ModificarSolicitudRequestMicroServ();
        private Transaccion dataTransaccion = new Transaccion();
        public ModificarSolicitudRequestMicroServ DataSolicitud { get => dataSolicitud; set => dataSolicitud = value; }
        public Transaccion DataTransaccion { get => dataTransaccion; set => dataTransaccion = value; }
    }

    public class SolicitudRequest
    {
        public string IdSolicitud { get; set; } = "";
        public string Identificacion { get; set; } = "";
    }
}
