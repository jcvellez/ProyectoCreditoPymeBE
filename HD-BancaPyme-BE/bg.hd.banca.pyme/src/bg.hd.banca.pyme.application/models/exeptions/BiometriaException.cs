using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.exeptions
{
    public class BiometriaException : BaseCustomException
    {
        public BiometriaException(string message = "Biometria Exeption", string description = "", int statuscode = 500) : base(message, description, statuscode)
        {

        }
        public BiometriaException(string message = "Biometria Exeption", string description = "", int statuscode = 500, string? source = null) : base(message, description, statuscode, source)
        {

        }
        public BiometriaException(string message = "Biometria Exeption", string description = "", int statuscode = 500, string? source = null, string? helpLink = null) : base(message, description, statuscode, source, helpLink)
        {

        }
    }
}
