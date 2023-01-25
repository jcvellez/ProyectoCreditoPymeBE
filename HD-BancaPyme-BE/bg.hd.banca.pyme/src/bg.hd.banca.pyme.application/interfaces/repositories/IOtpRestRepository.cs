using bg.hd.banca.pyme.domain.entities.otp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.interfaces.repositories
{
    public interface IOtpRestRepository
    {
        Task<OtpGenerarResponse> GenerarOtp(OtpGenerarRequest _request);

        Task<OtpValidarResponse> ValidarOtp(OtpValidarRequest request);
    }
}
