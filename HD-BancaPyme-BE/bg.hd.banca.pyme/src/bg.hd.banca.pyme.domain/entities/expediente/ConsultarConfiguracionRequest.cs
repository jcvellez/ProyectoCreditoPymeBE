using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultarConfiguracionRequest
    {
        string _identificacion = string.Empty;
        string _idTipoConfiguracionExpediente = string.Empty;
        string _idEtapa = string.Empty;
        string _idDictamen = string.Empty;
        string _estado = string.Empty;
        string _idProducto = string.Empty;
        string _idFormulario = string.Empty;
        string _idModulo = string.Empty;
        string _strUsuario = string.Empty;
        bool _generarException = true;

        public string idTipoConfiguracionExpediente { get => _idTipoConfiguracionExpediente; set => _idTipoConfiguracionExpediente = value; }
        public string idEtapa { get => _idEtapa; set => _idEtapa = value; }
        public string idDictamen { get => _idDictamen; set => _idDictamen = value; }
        public string estado { get => _estado; set => _estado = value; }
        public string idProducto { get => _idProducto; set => _idProducto = value; }
        public string idFormulario { get => _idFormulario; set => _idFormulario = value; }
        public string idModulo { get => _idModulo; set => _idModulo = value; }
        public string strUsuario { get => _strUsuario; set => _strUsuario = value; }
        public string Identificacion { get => _identificacion; set => _identificacion = value; }
        public bool GenerarException { get => _generarException; set => _generarException = value; }
    }
} 
