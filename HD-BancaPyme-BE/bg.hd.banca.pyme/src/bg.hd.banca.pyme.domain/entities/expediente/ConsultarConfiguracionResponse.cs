using bg.hd.banca.pyme.domain.entities.config;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ConsultarConfiguracionResponse
    {

        string _idCodigo = string.Empty;
        string _tipoConfiguracion = string.Empty;
        string _etapa = string.Empty;
        string _codProducto = string.Empty;
        string _producto = string.Empty;
        string _codDictamen = string.Empty;
        string _dictamen = string.Empty;
        string _idCatEstadoActualiza = string.Empty;
        string _codEstadoActualiza = string.Empty;
        string _EstadoActualiza = string.Empty;
        Transaccion dataTransaccion = new Transaccion();

        public string idCodigo { get => _idCodigo; set => _idCodigo = value; }
        public string tipoConfiguracion { get => _tipoConfiguracion; set => _tipoConfiguracion = value; }
        public string etapa { get => _etapa; set => _etapa = value; }
        public string codProducto { get => _codProducto; set => _codProducto = value; }
        public string producto { get => _producto; set => _producto = value; }
        public string codDictamen { get => _codDictamen; set => _codDictamen = value; }
        public string dictamen { get => _dictamen; set => _dictamen = value; }
        public string idCatEstadoActualiza { get => _idCatEstadoActualiza; set => _idCatEstadoActualiza = value; }
        public string codEstadoActualiza { get => _codEstadoActualiza; set => _codEstadoActualiza = value; }
        public string EstadoActualiza { get => _EstadoActualiza; set => _EstadoActualiza = value; }
        public Transaccion DataTransaccion { get => dataTransaccion; set => dataTransaccion = value; }
    }
}
