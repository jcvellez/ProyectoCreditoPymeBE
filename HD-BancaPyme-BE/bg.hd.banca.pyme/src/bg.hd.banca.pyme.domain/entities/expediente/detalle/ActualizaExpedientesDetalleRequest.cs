using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente.detalle
{
    public class ActualizaExpedientesDetalleRequest
    {
        public string idExpediente { get; set; } = "";
        public string idProducto { get; set; } = "";
        public string idCaptadoPor { get; set; } = "";
        public string idfondosProvienen { get; set; } = "";
        public string comentarioFondosProvienen { get; set; } = "";
        public string fondosDestinos { get; set; } = "";
        public string idAgenciaCliente { get; set; } = "";
        public string idModulo { get; set; } = "";
        public string idFormulario { get; set; } = "";
        public string strUsuario { get; set; } = "";
        public string fechaAceptacionContrato { get; set; } = "";
    }
}
