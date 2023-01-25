using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.catalogo
{
    public class ConsultarCatalogoRequestMicroServ
    {
        public int opcion { get; set; }
        //public string producto { get; set; }
        public string idCatalogo { get; set; }
        public string idCatalogoPadre { get; set; }
        public string Filtro { get; set; }
        public string valorFiltro { get; set; }

    }
}
