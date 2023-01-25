using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.documento
{
    public class DigitalizarDocumentosRequest
    {
        /// <summary>
        /// IdExpediente del expediente del cliente
        /// </summary>
        /// <example>9814184</example>
        [Required]
        public int? IdExpediente { get; set; }
        /// <summary>
        /// Usuario app banco
        /// </summary>
        /// <example>neoWeb</example>
        [JsonIgnore]
        public string? Usuario { get; set; }
        /// <summary>
        /// IdDocumento 
        /// </summary>
        /// <example>1556</example>
        [JsonIgnoreAttribute]
        public int? IdDocumento { get; set; }
        /// <summary>
        /// NombreDocumento : strNombreOnBase
        /// </summary>
        /// <example>DBP - ESTADOS DE CUENTA-CERTIFICADOS BANCARIOS NO BG-COMERCIALES</example>
        [JsonIgnoreAttribute]
        public string? NombreDocumento { get; set; }
        /// <summary>
        /// NumItem numero de Items 
        /// </summary>
        /// <example>1</example>
        public int? NumItem { get; set; }

        [Required]
        public string? Id { get; set; }

        [Required]
        public ListaDocumentos ListaDocumentos { get; set; }
        /// <summary>
        /// ProcesaImagen
        /// </summary>
        /// <example>true</example>
        [JsonIgnoreAttribute]
        public bool ProcesaImagen { get; set; }
        /// <summary>
        /// TamanioMaximoDocumento
        /// </summary>
        /// <example>4</example>
        [JsonIgnoreAttribute]
        public int TamanioMaximoDocumento { get; set; }

    }
    public class ListaDocumentos
    {
        [Required]
        public List<ListadoDocumentoDigitalizar> ListadoDocumentoDigitalizar { get; set; }

        public ListaDocumentos()
        {
            ListadoDocumentoDigitalizar = new List<ListadoDocumentoDigitalizar>();

        }
    }
    public class ListadoDocumentoDigitalizar
    {
        string _NombreArchivo = string.Empty;
        /// <summary>
        /// extension del documento
        /// </summary>
        /// <example>JPG</example>
        [JsonPropertyName("ExtDocumento")]
        public string ExtDocumento { get; set; }
        /// <summary>
        /// extension del documento
        /// </summary>
        [JsonPropertyName("NombreArchivo")]
        public string NombreArchivo { get => _NombreArchivo; set => _NombreArchivo = value; }
        /// <summary>
        /// byte[] archivo  --> pdf o imagen convertida en byte
        /// </summary>
        /// <example>/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoHCBYWFRgWFhYZGRgaGRwYGhwcHBwcHhocHBgaGhgaGh.......</example>
        [JsonPropertyName("Archivo")]
        public byte[] Archivo { get; set; }
    }
}
