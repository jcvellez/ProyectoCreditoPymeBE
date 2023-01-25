using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.ms
{
    public class MsResponse<T>
    {
        public bool? success { get; set; }
        public bool? collection { get; set; }
        public int? count { get; set; }
        public T? data { get; set; }
        public MsError? error { get; set; } 
    }
}
