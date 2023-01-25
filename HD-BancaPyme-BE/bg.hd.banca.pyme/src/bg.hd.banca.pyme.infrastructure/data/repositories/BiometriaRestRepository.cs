using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.biometria;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class BiometriaRestRepository : IBiometriaRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public BiometriaRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<RegistroBiometriaResponse> RegistroBiometria(RegistroBiometriaRequest request)
        {
            RegistroBiometriaResponse registroBiometriaResponse = new RegistroBiometriaResponse();
            MsDtoTiempoBloqueoError responseJsonErrorMsgTiempoBloqueo = new MsDtoTiempoBloqueoError();
            bool generarMsgTiempoBloqueo = false;
            string uri = "";

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.Producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                registroBiometriaResponse.logResp.codigoResponse = 2;
                registroBiometriaResponse.logResp.DescriptionResponse = "Producto ingresado no es correcto";
                registroBiometriaResponse.logResp.MessageResponse = "Producto ingresado no es correcto";
                PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request,  registroBiometriaResponse.logResp);
            }

            request.idProducto = producto.idProducto;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/solicitud/biometria/intento";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<RegistroBiometriaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<RegistroBiometriaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                registroBiometriaResponse = _mapper.Map<RegistroBiometriaResponse>(responseJson.data);
                registroBiometriaResponse.logResp.codigoResponse = 0;
                registroBiometriaResponse.logResp.DescriptionResponse = "Consulta exitosa";
                registroBiometriaResponse.logResp.MessageResponse = "Consulta exitosa";

            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {
                        MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        registroBiometriaResponse.logResp.MessageResponse = responseJsonError.errors[0].message.ToString();
                        registroBiometriaResponse.logResp.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";
                        registroBiometriaResponse.logResp.codigoResponse = responseJsonError.errors[0].code;
                        if (responseJsonError.errors[0].code == 3)
                        {
                            generarMsgTiempoBloqueo = true;
                            responseJsonErrorMsgTiempoBloqueo = JsonConvert.DeserializeObject<MsDtoTiempoBloqueoError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                            responseJsonErrorMsgTiempoBloqueo.errors[0].code = 6;
                            registroBiometriaResponse.logResp.MessageResponse = string.IsNullOrEmpty(responseJsonErrorMsgTiempoBloqueo.message) ? "Error Aplicativo ValidarBiometria" : responseJsonErrorMsgTiempoBloqueo.message.ToString();
                            registroBiometriaResponse.logResp.DescriptionResponse = responseJsonErrorMsgTiempoBloqueo.errors[0].message.ToString();
                            registroBiometriaResponse.logResp.codigoResponse = responseJsonErrorMsgTiempoBloqueo.errors[0].code;
                        }

                    }
                }
                else
                {
                    registroBiometriaResponse.logResp.MessageResponse = response.ReasonPhrase;
                    registroBiometriaResponse.logResp.DescriptionResponse = response.RequestMessage.ToString();
                    registroBiometriaResponse.logResp.codigoResponse = (int)response.StatusCode;
                }
                PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, registroBiometriaResponse.logResp);

                if (request.controlExcepcion) {
                    if (generarMsgTiempoBloqueo)
                    {
                        throw new BiometriaException(registroBiometriaResponse.logResp.MessageResponse, registroBiometriaResponse.logResp.DescriptionResponse, registroBiometriaResponse.logResp.codigoResponse, Convert.ToString(responseJsonErrorMsgTiempoBloqueo.errors[0].numeroIntentosRestantesBloqueo));

                    }
                    throw new GeneralException(registroBiometriaResponse.logResp.MessageResponse, registroBiometriaResponse.logResp.DescriptionResponse, registroBiometriaResponse.logResp.codigoResponse); 
                }

            }

            return registroBiometriaResponse;

        }

        public async Task<ValidaBiometriaResponse> ValidaBiometria(ValidaBiometriaRequest request)
        {
            ValidaBiometriaResponse validaBiometriaResponse = new ValidaBiometriaResponse();
            MsDtoTiempoBloqueoError responseJsonErrorMsgTiempoBloqueo = new MsDtoTiempoBloqueoError();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            bool generarMsgTiempoBloqueo = false;
            string uri = "";
 

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.Producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                logResp.codigoResponse = 2;
                logResp.DescriptionResponse = "Producto ingresado no es correcto";
                logResp.MessageResponse = "Producto ingresado no es correcto";
                PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, logResp);
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);
            }

            request.idProducto=producto.idProducto;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/solicitud/biometria/intento/validacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<ValidaBiometriaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ValidaBiometriaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                validaBiometriaResponse = _mapper.Map<ValidaBiometriaResponse>(responseJson.data);
                logResp.codigoResponse = validaBiometriaResponse.CodigoRetorno;
                logResp.DescriptionResponse = validaBiometriaResponse.Mensaje;
                logResp.MessageResponse = "Consulta exitosa";

            }
            else
            {
                generarException = true;
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {

                        MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        logResp.MessageResponse = responseJsonError.errors[0].message.ToString();
                        logResp.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";
                        logResp.codigoResponse = responseJsonError.errors[0].code;
                        if (responseJsonError.errors[0].code ==3)
                        {
                            generarMsgTiempoBloqueo = true;
                            responseJsonErrorMsgTiempoBloqueo = JsonConvert.DeserializeObject<MsDtoTiempoBloqueoError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                            responseJsonErrorMsgTiempoBloqueo.errors[0].code = 6;
                            logResp.MessageResponse = string.IsNullOrEmpty(responseJsonErrorMsgTiempoBloqueo.message) ? "Error Aplicativo ValidarBiometria" : responseJsonErrorMsgTiempoBloqueo.message.ToString();
                            logResp.DescriptionResponse = responseJsonErrorMsgTiempoBloqueo.errors[0].message.ToString();
                            logResp.codigoResponse = responseJsonErrorMsgTiempoBloqueo.errors[0].code;
                        }
                                               

                    }
                }
                else
                {
                    logResp.MessageResponse = response.ReasonPhrase;
                    logResp.DescriptionResponse = response.RequestMessage.ToString();
                    logResp.codigoResponse = (int)response.StatusCode;
                }
                PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion,  request, logResp);

                if (generarException) {
                    if (generarMsgTiempoBloqueo)
                    {
                        throw new BiometriaException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse, Convert.ToString(responseJsonErrorMsgTiempoBloqueo.errors[0].numeroIntentosRestantesBloqueo), Convert.ToString(responseJsonErrorMsgTiempoBloqueo.errors[0].tiempoBloqueoRestante));

                    }
                    throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); 
                }

            }

            return validaBiometriaResponse;

        }

        public async Task<ImagenTokenizadaResponse> GestionarBiometria(ImagenTokenizadaRequest request)
        {
            ImagenTokenizadaResponse imagenTokenizadaResponse = new ImagenTokenizadaResponse();
            //Transaccion logResp = new Transaccion();
           
            string uri = "";

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.Producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                imagenTokenizadaResponse.DataTransaccion.codigoResponse = 2;
                imagenTokenizadaResponse.DataTransaccion.DescriptionResponse = "Producto ingresado no es correcto";
                imagenTokenizadaResponse.DataTransaccion.MessageResponse = "Producto ingresado no es correcto";
                PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion,  request, imagenTokenizadaResponse.DataTransaccion);
                throw new GeneralException(imagenTokenizadaResponse.DataTransaccion.MessageResponse, imagenTokenizadaResponse.DataTransaccion.DescriptionResponse, imagenTokenizadaResponse.DataTransaccion.codigoResponse);
            }

            request.Aplicacion = producto.aplicacionValidaBiometria;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            uri = _configuration["InfraConfig:MicroClientes:urlAutorizaciones"] + "v1/biometria/imagen-tokenizada";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<ImagenTokenizadaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ImagenTokenizadaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                imagenTokenizadaResponse = _mapper.Map<ImagenTokenizadaResponse>(responseJson.data);
                imagenTokenizadaResponse.DataTransaccion.codigoResponse = 0;
                imagenTokenizadaResponse.DataTransaccion.DescriptionResponse = "Consulta exitosa";
                imagenTokenizadaResponse.DataTransaccion.MessageResponse = "Ok";

            }
            else
            {
               
                if ((int)response.StatusCode == 400 || (int)response.StatusCode == 401)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {

                        MsDtoErrorBiometriaPor responseJsonError = JsonConvert.DeserializeObject<MsDtoErrorBiometriaPor>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        imagenTokenizadaResponse.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                        imagenTokenizadaResponse.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";
                        imagenTokenizadaResponse.DataTransaccion.codigoResponse = responseJsonError.errors[0].code;
                        imagenTokenizadaResponse.PorcentajeCoincidencia = responseJsonError.errors[0].porcentajeCoincidencia.ToString();

                    }
                }
                else
                {
                    imagenTokenizadaResponse.DataTransaccion.MessageResponse = response.ReasonPhrase;
                    imagenTokenizadaResponse.DataTransaccion.DescriptionResponse = response.RequestMessage.ToString();
                    imagenTokenizadaResponse.DataTransaccion.codigoResponse = (int)response.StatusCode;
                }
                PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, imagenTokenizadaResponse.DataTransaccion);

                if (request.generarExcepcion) { throw new GeneralException(imagenTokenizadaResponse.DataTransaccion.MessageResponse, imagenTokenizadaResponse.DataTransaccion.DescriptionResponse, imagenTokenizadaResponse.DataTransaccion.codigoResponse); }

            }

            return imagenTokenizadaResponse;
        }

        public async Task<BiometriaTrazabilidadResponse> ConsultaInformacionTrazabilidad(BiometriaTrazabilidadRequest request)
        {
            BiometriaTrazabilidadResponse validaBiometriaResponse = new BiometriaTrazabilidadResponse();
            MsDtoTiempoBloqueoError responseJsonErrorMsgTiempoBloqueo = new MsDtoTiempoBloqueoError();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("idTrazabilidadNV", request.idTrazabilidadNV.ToString());
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();

            string uri = string.Format(_configuration["InfraConfig:MicroClientes:urlAutorizaciones"]) + "v1/biometria/trazabilidad";

            response = await client.GetAsync(uri);

            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<BiometriaTrazabilidadResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<BiometriaTrazabilidadResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                validaBiometriaResponse = _mapper.Map<BiometriaTrazabilidadResponse>(responseJson.data);
                logResp.codigoResponse = 0;
                logResp.DescriptionResponse = "OK";
                logResp.MessageResponse = "Consulta exitosa";

            }
            else
            {
                generarException = true;
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {

                        MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        logResp.MessageResponse = responseJsonError.errors[0].message.ToString();
                        logResp.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";
                        logResp.codigoResponse = responseJsonError.errors[0].code;

                    }
                }
                else
                {
                    logResp.MessageResponse = response.ReasonPhrase;
                    logResp.DescriptionResponse = response.RequestMessage.ToString();
                    logResp.codigoResponse = (int)response.StatusCode;
                }
                PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion,  request,  logResp);

                if (generarException)
                {
                    throw new BiometriaException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);
                }

            }

            return validaBiometriaResponse;

        }
    }
}
