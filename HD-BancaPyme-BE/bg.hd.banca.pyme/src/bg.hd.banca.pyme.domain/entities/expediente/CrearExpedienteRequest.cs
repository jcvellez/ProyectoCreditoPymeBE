namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class CrearExpedienteRequest
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0913521613</example>
        public string? Identificacion { get; set; }
        /// <summary>
        /// Id Solicitud
        /// </summary>
        /// <example>45908</example>
        public string? IdSolicitud { get; set; }
        /// <summary>
        /// Producto
        /// </summary>
        /// <example>cuotaMensual</example>
        public string? Producto { get; set; }
        /// <summary>
        /// MontoSolicitado
        /// </summary>
        /// <example>10000</example>
        public string MontoSolicitado { get; set; } = "0";
        /// <summary>
        /// PlazoSolicitado
        /// </summary>
        /// <example>30</example>
        public string PlazoSolicitado { get; set; } = "0";
        /// <summary>
        /// TipoTabla
        /// </summary>
        /// <example>fija</example>
        public string TipoTabla { get; set; } = "";
        /// <summary>
        /// TasaProducto
        /// </summary>
        /// <example>10.72</example>
        public string TasaProducto { get; set; } = "0";
        /// <summary>
        /// ValorDividendo
        /// </summary>
        /// <example>25.70</example>
        public string ValorDividendo { get; set; } = "0";
        /// <summary>
        /// DiaPago
        /// </summary>
        /// <example>1</example>
        public string DiaPago { get; set; } = "0";
        /// <summary>
        /// IdExpediente
        /// </summary>
        /// <example>0</example>
        public int? IdExpediente { get; set; }
        /// <example>KSANCHEZ2</example>

        public string UsuarioGestor { get; set; } = "";
        /// <example>KSO</example>

        public string OpidGestor { get; set; } = "";
        /// <example>2667</example>
        public string idAgenciaOficial { get; set; }
        public bool ClienteTokenizado { get; set; } = false;

    }
    

    public class IngresoExpedienteRequest
    {
        string _identificacion = string.Empty;
        string _idExpediente = "0";
        string _idPersona = string.Empty;
        string _idCatOficina = string.Empty;
        string _idOficina = string.Empty;
        string _usuarioGestor = string.Empty;
        string _usuarioEtapa = string.Empty;
        string _opidGestor = string.Empty;
        string _idProducto = string.Empty;
        string _idCanal = string.Empty;
        string _idformulario = string.Empty;
        string _idmodulo = string.Empty;
        string _strUsuario = string.Empty;
        string _observacion = string.Empty;
        string _descriptionProducto = string.Empty;

        public string identificacion { get => _identificacion; set => _identificacion = value; }
        public string idExpediente { get => _idExpediente; set => _idExpediente = value; }
        public string idPersona { get => _idPersona; set => _idPersona = value; }
        public string idCatOficina { get => _idCatOficina; set => _idCatOficina = value; }
        public string idOficina { get => _idOficina; set => _idOficina = value; }
        public string idProducto { get => _idProducto; set => _idProducto = value; }
        public string idCanal { get => _idCanal; set => _idCanal = value; }
        public string idFormulario { get => _idformulario; set => _idformulario = value; }
        public string idModulo { get => _idmodulo; set => _idmodulo = value; }
        public string usuarioGestor { get => _usuarioGestor; set => _usuarioGestor = value; }
        public string usuarioEtapa { get => _usuarioEtapa; set => _usuarioEtapa = value; }
        public string opidGestor { get => _opidGestor; set => _opidGestor = value; }
        public string strUsuario { get => _strUsuario; set => _strUsuario = value; }
        public string observacion { get => _observacion; set => _observacion = value; }
        public string descriptionProducto { get => _descriptionProducto; set => _descriptionProducto = value; }

    }
}
