using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ActualizaInformacionNormativaRequest
    {
        public string identificacionCliente { get; set; } = "";
        public string opcionActualizar { get; set; } = "";
        public string opcion { get; set; } = "";
        public string idExpediente { get; set; } = "";
        public string usuario { get; set; } = "";
        public string preguntaPaisNacimientoDiferenteEcuador { get; set; } = "";
        public string paisNacimiento { get; set; } = "";
        public string preguntaEstasCasadoUnionHecho { get; set; } = "";
        public string idTipoIdentificacionConyuge { get; set; } = "";
        public string identificacionConyuge { get; set; } = "";
        public string primerNombreConyuge { get; set; } = "";
        public string segundoNombreConyuge { get; set; } = "";
        public string apellidoPaternoConyuge { get; set; } = "";
        public string apellidoMaternoConyuge { get; set; } = "";
        public string preguntaTienesRucActivo { get; set; } = "";
        public string numeroRUC { get; set; } = "";
        public bool GenerarException { get; set; } = true;
    }
}
