using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.security
{
    public class AuthenticationResponse
    {
        public string? token_type { set; get; }
        public string? expires_in { set; get; }
        public string? ext_expires_in { set; get; }
        public string? expires_on { set; get; }
        public string? not_before { set; get; }
        public string? resource { set; get; }
        public string? access_token { set; get; }

        public DateTime? expired_token { set; get; }

        public string? error { set; get; }
        public string? error_description { set; get; }
        public string? timestamp { set; get; }
        public string? trace_id { set; get; }
        public string? correlation_id { set; get; }
        public string? error_uri { set; get; }
    }
}
