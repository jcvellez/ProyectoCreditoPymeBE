using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.otp;
using bg.hd.banca.pyme.application.models.exeptions;

namespace bg.hd.banca.pyme.application.services
{
    public class OtpRepository : IOtpRepository
    {
        private readonly IOtpRestRepository _otpRestRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public OtpRepository(IOtpRestRepository _otpRestRepository, ITokenGenerator _tokenGenerator)
        {
            this._otpRestRepository = _otpRestRepository;
            this._tokenGenerator = _tokenGenerator;
        }

        public async Task<OtpGenerarResponse> GenerarOtp(OtpGenerarRequest request)
        {
            Int64 ValAux = 0;
            if (Int64.TryParse(request.Identificacion, out ValAux) == false)
            {
                throw new IdentificarUsuarioException("Identificación debe ser numérica", "Identificación debe ser numérica", 2);
            }

            if (request.Identificacion.Length < 10 || request.Identificacion.Length > 10)
            {
                throw new IdentificarUsuarioException("Identificación no tiene formato solicitado de 10 caracteres", "Identificación no tiene formato solicitado de 10 caracteres", 2);
            }

            if (request.Identificacion.Trim() == "" || request.Identificacion is null)
            {
                throw new IdentificarUsuarioException("Identificación es requerida", "Identificación es requerida", 2);
            }
            return await _otpRestRepository.GenerarOtp(request);
        }

       public async Task<OtpValidarResponse> ValidarOtp(OtpValidarRequest request)
        {
            #region validaCampoIdentificacion
            Int64 ValAux = 0;
            if (Int64.TryParse(request.identificacion, out ValAux) == false)
            {
                throw new OtpValidarExeption("Identificación debe ser numérica", "Identificación debe ser numérica", 2);
            }

            if (request.identificacion.Length < 10 || request.identificacion.Length > 10)
            {
                throw new OtpValidarExeption("Identificación no tiene formato solicitado de 10 caracteres", "Identificación no tiene formato solicitado de 10 caracteres", 2);
            }

            if (request.identificacion.Trim() == "" || request.identificacion is null)
            {
                throw new OtpValidarExeption("Identificación es requerida", "Identificación es requerida", 2);
            }
            #endregion
            #region validaCampoOTP
            Int64 ValAux2 = 0;
            if (Int64.TryParse(request.otp, out ValAux2) == false)
            {
                throw new OtpValidarExeption("otp debe ser numérica", "otp debe ser numérica", 2);
            }

            if (request.otp.Length < 6 || request.otp.Length > 6)
            {
                throw new OtpValidarExeption("otp no tiene formato solicitado de 6 caracteres", "otp no tiene formato solicitado de 6 caracteres", 2);
            }

            if (request.otp.Trim() == "" || request.otp is null)
            {
                throw new OtpValidarExeption("otp es requerida", "otp es requerida", 2);
            }
            #endregion
            OtpValidarResponse response = await _otpRestRepository.ValidarOtp(request);
            if (response.CodigoRetorno == 0)
                response.jwtCliente = _tokenGenerator.GenerateTokenJwt(request.identificacion, out DateTime _expireTime, out int _expireIn);
            return response;
        }
    }
}
