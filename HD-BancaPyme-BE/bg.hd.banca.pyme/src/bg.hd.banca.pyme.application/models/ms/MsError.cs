using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.ms
{
    public class MsError
    {
        public int? status { set; get; }
        public string? errorCode { set; get; }
        public string? userMessage { set; get; }
        public string? traceId { set; get; } 
    }
}