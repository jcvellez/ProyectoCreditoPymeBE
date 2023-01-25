using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IAuthenticationServiceRepository
    {
        Task<string> GetAccessToken();
    }
}
