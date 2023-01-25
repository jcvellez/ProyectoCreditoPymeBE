using bg.hd.banca.pyme.domain.entities.config;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ValidarPoliticasResponse
    {
        string _idDictamen = string.Empty;
        string _dictamen = string.Empty;
        string _motivoDictamen = string.Empty;
        string _tipoGarantia = string.Empty;
        string _idGarantiaSegmentoCliente = string.Empty;
        Transaccion dataTransaccion = new Transaccion();

        public string idDictamen { get => _idDictamen; set => _idDictamen = value; }
        public string dictamen { get => _dictamen; set => _dictamen = value; }
        public string motivoDictamen { get => _motivoDictamen; set => _motivoDictamen = value; }
        public string tipoGarantia { get => _tipoGarantia; set => _tipoGarantia = value; }
        public string idGarantiaSegmentoCliente { get => _idGarantiaSegmentoCliente; set => _idGarantiaSegmentoCliente = value; }
        public Transaccion DataTransaccion { get => dataTransaccion; set => dataTransaccion = value; }
    }
}
