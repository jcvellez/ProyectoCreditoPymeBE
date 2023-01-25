using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.exeptions
{
    public class EmailValidacionExecption : BaseCustomException
    {
        public EmailValidacionExecption(string message = "Email Validacion Exception", string description = "", int statuscode = 500) : base(message, description, statuscode)
        {

        }
    }
}
