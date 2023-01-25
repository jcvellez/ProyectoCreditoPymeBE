using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities
{
    public class FlujoExpediente
    {
        private string _id = string.Empty;
        private string _idDescription = string.Empty;

        public string id { get => _id; set => _id = value; }
        public string idDescription { get => _idDescription; set => _idDescription = value; }
    }

    public class Flujo
    {
        string _autenticacion = string.Empty;
        string _idEstado = string.Empty;
        string _idEtapa = string.Empty;
        string _redireccionar = string.Empty;

        public string autenticacion { get => _autenticacion; set => _autenticacion = value; }
        public string idEstado { get => _idEstado; set => _idEstado = value; }
        public string idEtapa { get => _idEtapa; set => _idEtapa = value; }
        public string redireccionar { get => _redireccionar; set => _redireccionar = value; }
    }
}