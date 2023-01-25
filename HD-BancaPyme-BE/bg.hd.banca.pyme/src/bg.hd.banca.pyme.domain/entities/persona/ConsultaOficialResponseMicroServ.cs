using bg.hd.banca.pyme.domain.entities.config;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class ConsultaOficialResponseMicroServ
    {
        private string _usuarioOficial = string.Empty;
        private string _opidOficial = string.Empty;
        private string _idAgenciaOficial = string.Empty;
        private string _usuarioJefeAencia = string.Empty;
        private string _opidJefeAgencia = string.Empty;
        private bool _oficialExiste = false;
        private Transaccion dataTransaccion = new Transaccion();
        public string usuarioOficial { get => _usuarioOficial; set => _usuarioOficial = value; }
        public string opidOficial { get => _opidOficial; set => _opidOficial = value; }
        public string idAgenciaOficial { get => _idAgenciaOficial; set => _idAgenciaOficial = value; }
        public string usuarioJefeAencia { get => _usuarioJefeAencia; set => _usuarioJefeAencia = value; }
        public string opidJefeAgencia { get => _opidJefeAgencia; set => _opidJefeAgencia = value; }
        public bool oficialExiste { get => _oficialExiste; set => _oficialExiste = value; }
        public Transaccion DataTransaccion { get => dataTransaccion; set => dataTransaccion = value; }
    }
}
