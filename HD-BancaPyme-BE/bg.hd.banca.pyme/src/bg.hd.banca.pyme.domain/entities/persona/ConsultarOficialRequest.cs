using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultarOficialRequest
    {
        private string _identificacion = string.Empty;
        private int _idTipoIdentificacion = 0;
        private int _canal = 0;

        public string identificacion { get => _identificacion; set => _identificacion = value; }
        public int tipoIdentificacion { get => _idTipoIdentificacion; set => _idTipoIdentificacion = value; }
        public int canal { get => _canal; set => _canal = value; }
    }
}
