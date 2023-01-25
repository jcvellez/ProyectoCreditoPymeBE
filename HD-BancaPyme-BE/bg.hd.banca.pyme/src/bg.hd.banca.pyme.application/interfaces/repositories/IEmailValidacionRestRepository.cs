using bg.hd.banca.pyme.domain.entities.email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IEmailValidacionRestRepository
    {
        Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request);
    }
}
