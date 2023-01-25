using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.pyme.domain.entities.recaptcha;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IRecaptchaRestRepository
    {
        Task<RecaptchaResponse> validarRecaptcha(RecaptchaRequest request);
    }
}
