using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.persona.domain.entities
{
    public class Transaccion
    {
        private string messageResponse = string.Empty;
        private string descriptionResponse = string.Empty;
        public int codigoResponse { get; set; }
        public string MessageResponse { get => this.messageResponse; set => this.messageResponse = value; }
        public string DescriptionResponse { get => this.descriptionResponse; set => this.descriptionResponse = value; }
    }
}
