namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class Activos
    {
        public string? IdTipoPeriodicidad { get; set; }
        public string? IdTipoTabla { get; set; }
        public string? IdGarantiaCredito { get; set; }
        public string? IdSubtipoGarantiaCredito { get; set; }
        public string? IdNormativa { get; set; } // equivale al idBinAfinidadTarjeta de tarjeta de crédito
        public string? IdTipoCuentaCredito{ get; set; }
        public string? IdLugarEntregaTarjeta{ get; set; }
        public string? PlazoSolicitado { get; set; }
        public string? DiaPago{ get; set; }
        public string? MontoSolicitado { get; set; }
        public string? TasaProducto { get; set; }     
        public string? ValorDividendo{ get; set; }
        public string? PropositoCredito{ get; set; }
        public string? IdNumCuentaCredito{ get; set; }
        public string? NombreEnTarjeta{ get; set; } // Se utiliza para MC como nombre de tarjeta debito y para TC como nombre en tarjeta crédito
        public string? CupoSolicitado{ get; set; } 
        public string? RequiereAvisoSeguro{ get; set; }
        public string? IdTipoValidacionBiometrica { get; set; }
        public string? CapacidadEndeudamientoMensual { get; set; }
        public string? MaximoDeudaConsumo { get; set; }
        public string? MaximaDeudaTc { get; set; }
        public string? IngresoFinal { get; set; }
        public string? SubProductoCore { get; set; }
        public string? IdSegmentoEstrategicoCore { get; set; }
        public string? PerfilCliente { get; set; }
        public string? SecuenciaRiesgo { get; set; }
    }
}
