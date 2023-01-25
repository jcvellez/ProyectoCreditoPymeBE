using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.recaptcha
{
    public class RecaptchaRequest
    {
        /// <summary>
        /// Google ReCaptcha Encoded Response.- OBLIGATORIO; Token generado por el widget de reCAPTCHA y que debe ser capturado con Request["g-recaptcha-response"].
        /// </summary>
        /// <example>6LcmrXUUAAAAAJne55MlrLcn4bCI-Jn14aKxbjOC</example>
        public string? token { get; set; }
        /// <summary>
        /// dirección IP/dominio del cliente, servirá para validar el dominio en caso de que en la configuración de Google ReCaptcha se deje habilitado “validar dominio”
        /// </summary>
        /// <example>https://apps.bancoguayaquil.com/reclutamiento/</example>
        public string? remoteIp { get; set; }
    }
}
