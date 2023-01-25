using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.otp;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class OtpRestRepository : IOtpRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public OtpRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }


        #region "Método Generar OTP 31/03/2022 ts-lagurto"

        public async Task<OtpGenerarResponse> GenerarOtp(OtpGenerarRequest _request)
        {
            var mensaje = string.Empty;
            if (_configuration["GeneralConfig:envioNotificacionOtp"]!=null && _configuration["GeneralConfig:envioNotificacionOtp"].Trim().ToLower() == "sms")
            {
                using (var sr = new StreamReader("./templates/notificaciones/SmsNotificationTemplate.txt"))
                {
                    mensaje = sr.ReadToEnd();
                }
            }
            OtpGenerarResponse otpGenerarResponse = new OtpGenerarResponse();

            OtpGenerarRequestMicroServ requestMicro = new OtpGenerarRequestMicroServ()
            {
                Identificacion = _request.Identificacion,
                TipoIdentificacion = "C",
                Notificacion = _configuration["GeneralConfig:envioNotificacionOtp"],
                LlaveOTP = _configuration["llaveOTP"],
                IvOTP = _configuration["ivOTP"],
                Aplicacion = _configuration["GeneralConfig:aplicacionOTP"],
                Servicio = _configuration["GeneralConfig:servicioOTP"],
                Canal = _configuration["GeneralConfig:canalOTP"],
                OpidOTP = _configuration["GeneralConfig:opidOTP"],
                Terminal = _configuration["GeneralConfig:terminalOTP"],
                SmsOpid = _configuration["GeneralConfig:smsOpid"],
                SmsOrigen = _configuration["GeneralConfig:smsOpid"],
                Template = mensaje
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroClientes:urlAutorizaciones"]) + "v1/otp";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();
            MsResponse<OtpGenerarResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<OtpGenerarResponse>>(responseBody, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (response.IsSuccessStatusCode)
            {
                otpGenerarResponse = _mapper.Map<OtpGenerarResponse>(responseJson.data);

            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {

                        MsDtoResponseError responseJson1 = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });


                        string mensajeError; int cont = 0; int code = 2;
                        if (responseJson1.errors.Count == 1)
                        {
                            code = responseJson1.errors[0].code;
                            mensajeError = responseJson1.errors[0].message;
                        }
                        else
                        {
                            mensajeError = "";
                            foreach (MsDtoError error in responseJson1.errors)
                            {
                                cont += 1;
                                code = error.code;
                                mensajeError += $"Error {code}: {error.message} \n";
                            }
                        }
                        throw new OtpValidarExeption(mensajeError, mensajeError, code);


                    }
                    throw new OtpValidarExeption(response.ReasonPhrase, response.RequestMessage.ToString(), ((int)response.StatusCode));
                }
                else
                {
                    throw new OtpValidarExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, _request.Identificacion, _request, otpGenerarResponse);

            return otpGenerarResponse;



        }
        #endregion

        public async Task<OtpValidarResponse> ValidarOtp(OtpValidarRequest request)
        {

            Log.Information("{Proceso} {Canal}", "ValidarOtp IN", "Canal");

            OtpValidarResponse generarResponse = new OtpValidarResponse();

            OtpValidarRequestMicroServ requestMicroServ = new OtpValidarRequestMicroServ()
            {
                identificacion = request.identificacion,
                tipoIdentificacion = "C",
                otp = request.otp,
                llaveOTP = _configuration["llaveOTP"],
                ivOTP = _configuration["ivOTP"],
                aplicacion = _configuration["GeneralConfig:aplicacionOTP"],
                servicio = _configuration["GeneralConfig:servicioOTP"],
                canal = _configuration["GeneralConfig:canalOTP"],
                opidOTP = _configuration["GeneralConfig:opidOTP"],
                terminal = _configuration["GeneralConfig:terminalOTP"]
            };


            #region validaCampoOTP
            Int64 ValAux2 = 0;
            if (Int64.TryParse(requestMicroServ.otp, out ValAux2) == false)
            {
                throw new OtpValidarExeption("otp debe ser numérica", "otp debe ser numérica", 2);
            }

            if (requestMicroServ.otp.Length < 6 || requestMicroServ.otp.Length > 6)
            {
                throw new OtpValidarExeption("otp no tiene formato solicitado de 6 caracteres", "otp no tiene formato solicitado de 6 caracteres", 2);
            }

            if (requestMicroServ.otp.Trim() == "" || requestMicroServ.otp is null)
            {
                throw new OtpValidarExeption("otp es requerida", "otp es requerida", 2);
            }
            #endregion
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            var response = new HttpResponseMessage();

            string uri = string.Format(_configuration["InfraConfig:MicroClientes:urlAutorizaciones"]) + "v1/otp/validacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicroServ), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<OtpValidarResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<OtpValidarResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                generarResponse.CodigoRetorno = 0;
                generarResponse.Mensaje = "Validacion OTP Exitosa";



            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {

                        MsDtoResponseError responseJson = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });


                        string mensajeError; int cont = 0; int code = 2;
                        if (responseJson.errors.Count == 1)
                        {
                            code = responseJson.errors[0].code;
                            mensajeError = responseJson.errors[0].message;
                        }
                        else
                        {
                            mensajeError = "";
                            foreach (MsDtoError error in responseJson.errors)
                            {
                                cont += 1;
                                code = error.code;
                                mensajeError += $"Error {code}: {error.message} \n";
                            }
                        }
                        throw new OtpValidarExeption(mensajeError, mensajeError, code);


                    }
                    throw new OtpValidarExeption(response.ReasonPhrase, response.RequestMessage.ToString(), ((int)response.StatusCode));
                }
                else
                {
                    throw new OtpValidarExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, generarResponse);

            return generarResponse;
        }

    }    
}




