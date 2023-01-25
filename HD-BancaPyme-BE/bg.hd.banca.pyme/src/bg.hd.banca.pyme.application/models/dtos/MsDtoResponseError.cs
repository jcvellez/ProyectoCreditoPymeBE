using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.dtos
{
    public class MsDtoResponseError
    {
        public int code { get; set; }
        public string traceid { get; set; }
        public string message { get; set; }
        public List<MsDtoError> errors { get; set; }

    }
}
