using System.Collections.Generic;

namespace bg.hd.banca.pyme.domain.entities
{
    public class Producto
    {
        string _idProductoPadre = string.Empty;
        string _idProducto = string.Empty;
        string _nombreProducto = string.Empty;
        string _envioNotificacionOTP = string.Empty;
        string _tipoPeriodicidad = string.Empty;
        string _diasVigencia = string.Empty;
        List<Flujo> _flujo = new List<Flujo>();

        string _subProductoCore = string.Empty;
        public string idProductoPadre { get => _idProductoPadre; set => _idProductoPadre = value; }
        public string idProducto { get => _idProducto; set => _idProducto = value; }
        public string nombreProducto { get => _nombreProducto; set => _nombreProducto = value; }
        public string envioNotificacionOTP { get => _envioNotificacionOTP; set => _envioNotificacionOTP = value; }
        public string tipoPeriodicidad { get => _tipoPeriodicidad; set => _tipoPeriodicidad = value; }
        public string subProductoCore { get => _subProductoCore; set => _subProductoCore = value; }
        public string diasVigencia { get => _diasVigencia; set => _diasVigencia = value; }
        public string[] catalogosPermitidos { get; set; }
        public int[] opcionesValidas { get; set; }
        public string[] etapasProceso { get; set; }
        public string[] etapasAprobado { get; set; }
        public string[] etapasLiquidacion { get; set; }
        public string aplicacionValidaBiometria { get; set; } = "";


        public List<Flujo> flujo { get => _flujo; set => _flujo = value; }
    }
}
