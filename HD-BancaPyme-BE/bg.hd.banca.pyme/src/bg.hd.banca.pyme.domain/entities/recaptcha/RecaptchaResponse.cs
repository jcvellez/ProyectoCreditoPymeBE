using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.recaptcha
{
    public class RecaptchaResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        [JsonIgnore] public string sessionId { get; set; }

        [JsonIgnore] public bool success { get; set; }

        [JsonIgnore] public string challenge_ts { get; set; }

        [JsonIgnore] public string hostname { get; set; }

        [JsonIgnore] public string errorCodes { get; set; }
    }
}
