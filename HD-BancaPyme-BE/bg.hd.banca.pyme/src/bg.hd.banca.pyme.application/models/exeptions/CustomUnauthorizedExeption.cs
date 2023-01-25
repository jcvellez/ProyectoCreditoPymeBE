using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.exeptions
{
    public class CustomUnauthorizedExeption: BaseCustomException
    {
        public CustomUnauthorizedExeption(string message = "Unauthorized", string description = "", int statuscode = 401) : base(message, description, statuscode)
        {

        }
    }
}
