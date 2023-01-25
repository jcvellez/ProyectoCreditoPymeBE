using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class IdentificaUsuarioRequest
    {
        [JsonIgnore]
        public string? ClienteId { get; set; }

        [JsonIgnore]
        public string? TipoId { get; set; }

    }
}
