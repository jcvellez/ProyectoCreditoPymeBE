using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.biometria;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities.expediente;
using Microsoft.Extensions.Configuration;

namespace bg.hd.banca.pyme.application.services
{
    public class BiometriaRepository : IBiometriaRepository
    {
        private readonly IBiometriaRestRepository _biometriaRestRepository;
        private readonly IGestionExpedienteRestRepository _expedienteRestRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;


        public BiometriaRepository(IBiometriaRestRepository biometriaRestRepository, IGestionExpedienteRestRepository expedienteRestRepository,
            IConfiguration _configuration, ITokenGenerator _tokenGenerator)
        {
            this._biometriaRestRepository = biometriaRestRepository;
            this._expedienteRestRepository = expedienteRestRepository;
            this._configuration = _configuration;
            this._tokenGenerator = _tokenGenerator;
        }
        public async Task<RegistroBiometriaResponse> RegistroBiometria(RegistroBiometriaRequest request)
        {
            return await _biometriaRestRepository.RegistroBiometria(request);
        }

        public async Task<ValidaBiometriaResponse> ValidaBiometria(ValidaBiometriaRequest request)
        {
            return await _biometriaRestRepository.ValidaBiometria(request);
        }

        public async Task<ImagenTokenizadaResponse> GestionarBiometria(ImagenTokenizadaRequest request)
        {
            ImagenTokenizadaResponse responseTokenizada = new();
            RegistroBiometriaRequest requestRegistro = new();
            RegistroBiometriaResponse responseRegistro = new();
            ModificarSolicitudRequest requestSolicitud = new();
            ModificarSolicitudResponse responseSolicitud = new();
            BiometriaTrazabilidadResponse responseBiometriaTrazabilidad = new BiometriaTrazabilidadResponse();
            BiometriaTrazabilidadRequest requestBiometriaTrazabilidad = new BiometriaTrazabilidadRequest();

            request.generarExcepcion = false;
            responseTokenizada = await _biometriaRestRepository.GestionarBiometria(request);

            requestBiometriaTrazabilidad.identificacion = request.Identificacion;
            requestBiometriaTrazabilidad.idTrazabilidadNV = responseTokenizada.TrazabilidadNV;
            if (responseTokenizada.DataTransaccion.codigoResponse == 0) responseBiometriaTrazabilidad = await _biometriaRestRepository.ConsultaInformacionTrazabilidad(requestBiometriaTrazabilidad);
             
            #region Registro Biometria
            requestRegistro.Producto = request.Producto;
            requestRegistro.idSolicitud = request.IdSolicitud;
            requestRegistro.usuario = string.Format(_configuration["GeneralConfig:usuario"]);          
            switch (responseTokenizada.DataTransaccion.codigoResponse) 
            {
                case 0:
                    requestRegistro.estadoValidacion = "V";
                    break;
                case 2:
                    requestRegistro.estadoValidacion = "N";
                    break;
                default:
                    requestRegistro.estadoValidacion = "E";
                    break;
            }
            requestRegistro.controlExcepcion = false;
            requestRegistro.Identificacion = request.Identificacion;
            responseRegistro = await _biometriaRestRepository.RegistroBiometria(requestRegistro);
            #endregion

            #region ModificarSolicitud 
            requestSolicitud.Identificacion = request.Identificacion;
            requestSolicitud.Producto = request.Producto;
            requestSolicitud.IdSolicitud = request.IdSolicitud;
            requestSolicitud.FechaReconocimientoBiometrico = DateTime.Today.ToString("dd-MM-yyyy"); //DateTime.UtcNow.ToString();
            requestSolicitud.PorcentajeReconocimientoBiometrico = responseTokenizada.PorcentajeCoincidencia;
            requestSolicitud.RespuestaReconocimientoBiometrico = responseTokenizada.DataTransaccion.MessageResponse;
            requestSolicitud.IdTipoValidacionBiometrica = string.Format(_configuration["GeneralConfig:tipoValidacionBiometria"]);
            requestSolicitud.NavegadorWeb = request.NavegadorWeb;
            requestSolicitud.VersionSoOrigen = request.VersionOrigen;
            requestSolicitud.DispositvoOrigen = request.DispositivoOrigen;
            requestSolicitud.TieneCamaraWeb = request.TieneCamara;
            requestSolicitud.ImagenReconocimientoBiometrico = responseBiometriaTrazabilidad.fotoTomada;
            requestSolicitud.GenerarException = false;
            requestSolicitud.DireccionIp = request.IpCliente;
            responseSolicitud = await _expedienteRestRepository.ModificarSolicitud(requestSolicitud);
            #endregion



            if (responseTokenizada.DataTransaccion.codigoResponse == 0 && responseRegistro.CodigoRetorno == 0 && responseSolicitud.CodigoRetorno == 0)
            {
                responseTokenizada.jwtCliente = _tokenGenerator.GenerateTokenJwt(request.Identificacion, out DateTime _expireTime, out int _expireIn);

            }
            else { 

                TransaccionBiometria data = new TransaccionBiometria();
                data.codigoResponse = 1;
                data.MessageResponse = "Error";
                data.DescriptionResponse = "Aplicativo";
                data.numeroIntentosFallidos = responseRegistro.numeroIntentosFallidos;
                data.tiempoBloqueoRestante = responseRegistro.tiempoBloqueoRestante;
                data.numeroIntentosRestantesBloqueo = responseRegistro.numeroIntentosRestantesBloqueo;

                if (responseTokenizada.DataTransaccion.codigoResponse == 2)
                {
                    data.codigoResponse = 2;
                    data.MessageResponse = "Intento de autenticación no válido";
                    data.DescriptionResponse = "No es la misma persona";
                }
                if (responseRegistro.logResp.codigoResponse == 6)
                {
                    data.codigoResponse = 4;
                    data.MessageResponse = "Bloqueado";
                    data.DescriptionResponse = "Máximo de intentos fallidos alcanzado";
                    throw new BiometriaException(data.MessageResponse, data.DescriptionResponse, data.codigoResponse, Convert.ToString(data.numeroIntentosRestantesBloqueo));

                }
                throw new BiometriaException(data.MessageResponse, data.DescriptionResponse, data.codigoResponse, Convert.ToString(data.numeroIntentosRestantesBloqueo));
            }
            return responseTokenizada;
        }
    }
}
