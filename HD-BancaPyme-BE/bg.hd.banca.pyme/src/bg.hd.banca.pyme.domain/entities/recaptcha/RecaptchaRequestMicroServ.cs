using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.recaptcha
{
    public class RecaptchaRequestMicroServ
    {
        public string? secretKey { get; set; }
        public string? token { get; set; }
        public string? remoteIp { get; set; }
    }
}
