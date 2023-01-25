using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.documento
{
    public class Documento
    {
        string _id = string.Empty;
        string _idDocumento = string.Empty;
        string _nombreDocumento = string.Empty;
        string _tamanioMaximoDocumento = string.Empty;
        string _procesaImagen = string.Empty;   

        public string id { get => _id; set => _id = value; }
        public string idDocumento { get => _idDocumento; set => _idDocumento = value; }
        public string nombreDocumento { get => _nombreDocumento; set => _nombreDocumento = value; }
        public string tamanioMaximoDocumento { get => _tamanioMaximoDocumento; set => _tamanioMaximoDocumento = value; }
        public string procesaImagen { get => _procesaImagen; set => _procesaImagen = value; }
    }
}
