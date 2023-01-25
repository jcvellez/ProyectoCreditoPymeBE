using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ModificarSolicitudRequest
    { 
        [Required] public string IdSolicitud { get; set; }

        [Required] public string Producto { get; set; }

        [Required] public string Identificacion { get; set; }

        public string? MontoSolicitado { get; set; }

        public string? PlazoSolicitado { get; set; }

        public string? TipoTabla { get; set; }

        public string? TasaProducto { get; set; }

        public string? ValorDividendo { get; set; }

        public string? ImagenReconocimientoBiometrico { get; set; }

        public string? PorcentajeReconocimientoBiometrico { get; set; }

        public string? RespuestaReconocimientoBiometrico { get; set; }

        public string? FechaReconocimientoBiometrico { get; set; }

        public string? NavegadorWeb { get; set; }

        public string? DispositvoOrigen { get; set; }

        public string? VersionSoOrigen { get; set; }

        public string? TieneCamaraWeb { get; set; }

        public string? DireccionIp { get; set; }

        public string? DiaPago { get; set; }
 
        public string? CodigoCampania { get; set; }

        public string? IdTipoValidacionBiometrica { get; set; }

        public string? CapacidadEndeudamientoMensual { get; set; }

        public string? MaximoDeudaConsumo { get; set; }

        public string? MaximaDeudaTc { get; set; }

        public string? IngresoFinal { get; set; }

        public string? SubProductoCore { get; set; }

        public string? IdSegmentoEstrategicoCore { get; set; }

        public string? PerfilCliente { get; set; }

        public string? SecuenciaRiesgo { get; set; }

        public string? IdExpediente { get; set; }

        public string? Nombre { get; set; }

        public string? CorreoCore { get; set; }

        public string? CelularCore { get; set; }

        public string? MontoTotalCredito { get; set; }
        public string? idTipoPeriodicidad { get; set; }
        public string? idTipoTabla { get; set; }
        public string? idEstadoActualSolicitud { get; set; }
        public string? tieneActVenta { get; set; } = "";
        public string? tieneActDirNegocio { get; set; } = "";
        public string? fuenteIngreso { get; set; } = "";


        bool generarException = true;

        [JsonIgnore]
        public bool GenerarException { get => generarException; set => generarException = value; }

    }
}
