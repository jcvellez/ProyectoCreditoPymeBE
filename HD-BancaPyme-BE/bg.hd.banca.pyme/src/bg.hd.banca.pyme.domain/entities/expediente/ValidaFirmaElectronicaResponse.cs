using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ValidaFirmaElectronicaResponse
    {
        public bool excedeMaximoPermitido { get; set; } = false;
        public string codError { get; set; } = "";
        public string msgError { get; set; } = "";
    }
}
