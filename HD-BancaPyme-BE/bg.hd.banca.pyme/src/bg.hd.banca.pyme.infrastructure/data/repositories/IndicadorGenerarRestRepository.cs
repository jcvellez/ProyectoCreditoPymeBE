using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.indicador;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    public class IndicadorGenerarRestRepository : IIndicadorGenerarRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public IndicadorGenerarRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<IndicadorGenerarResponse> IndicadorGenerar(IndicadorGenerarRequest request)
        {

            IndicadorGenerarResponse indicadorResponse = new IndicadorGenerarResponse();
            request.tipoIdentificacion = "C";
            request.usuario = _configuration["GeneralConfig:usuarioWeb"];
            #region CrearAnalisisRating
            var indicadorMicro = new HttpClient();
            indicadorMicro.DefaultRequestHeaders.Accept.Clear();
            indicadorMicro.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            indicadorMicro.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());            

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/indicador";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            responseMicro = await indicadorMicro.PostAsync(uri, httpContent);
            string responseBodyMicro = await responseMicro.Content.ReadAsStringAsync();
            MsResponse<IndicadorGenerarResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<IndicadorGenerarResponse>>(responseBodyMicro, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });
            
            if (responseMicro.IsSuccessStatusCode)
            {
                indicadorResponse = _mapper.Map<IndicadorGenerarResponse>(responseJson.data);                     
            }
            else
            {
                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseBodyMicro.Contains("code") && responseBodyMicro.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBodyMicro, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });
                        string mensajeError; int cont = 0; int code = 2;
                        if (responseJsonMicro.errors.Count == 1)
                        {
                            code = responseJsonMicro.errors[0].code;
                            mensajeError = responseJsonMicro.errors[0].message;
                        }
                        else
                        {
                            mensajeError = "";
                            foreach (MsDtoError error in responseJsonMicro.errors)
                            {
                                cont += 1;
                                code = error.code;
                                mensajeError += $"Error {code}: {error.message} \n";
                            }
                        }

                        throw new IndicadorGenerarException(mensajeError, mensajeError, code);

                    }                    
                }
                else

                    throw new IndicadorGenerarException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            #endregion
            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, indicadorResponse);

            return indicadorResponse;          

            //IndicadorGenerarResponse obj = new IndicadorGenerarResponse();
            //obj.Mensaje = "Validando";
            //return obj;
        }
        public async Task<GarantiaActualizarResponse> GarantiaActualizar(IndicadorGenerarRequest request)
        {
            GarantiaActualizarResponse garantiaResponse = new GarantiaActualizarResponse();

            GarantiaActualizarRequest request_garantia = new GarantiaActualizarRequest();
            request_garantia.idProceso = request.idProceso;
            request_garantia.identificacion = request.identificacion;
            request_garantia.tipoIdentificacion = request.tipoIdentificacion;

            var garantiaMicro = new HttpClient();
            garantiaMicro.DefaultRequestHeaders.Accept.Clear();
            garantiaMicro.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            garantiaMicro.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            var responsegarantiaMicro = new HttpResponseMessage();
            string uri_garantia = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/garantia-actualizar";
            HttpContent httpContent_garantia = new StringContent(JsonConvert.SerializeObject(request_garantia), Encoding.UTF8);
            httpContent_garantia.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            responsegarantiaMicro = await garantiaMicro.PostAsync(uri_garantia, httpContent_garantia);
            string responseBodygarantiaMicro = await responsegarantiaMicro.Content.ReadAsStringAsync();
            MsResponse<GarantiaActualizarResponse> responseJson_garantia = JsonConvert.DeserializeObject<MsResponse<GarantiaActualizarResponse>>(responseBodygarantiaMicro, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responsegarantiaMicro.IsSuccessStatusCode)
            {
                garantiaResponse = _mapper.Map<GarantiaActualizarResponse>(responseJson_garantia.data);
                garantiaResponse.CodigoRetorno = 0;
                garantiaResponse.Mensaje = garantiaResponse.Mensaje;
            }
            else
            {
                if ((int)responsegarantiaMicro.StatusCode == 400)
                {
                    if (responseBodygarantiaMicro.Contains("code") && responseBodygarantiaMicro.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBodygarantiaMicro, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });
                        string mensajeError; int cont = 0; int code = 2;
                        if (responseJsonMicro.errors.Count == 1)
                        {
                            code = responseJsonMicro.errors[0].code;
                            mensajeError = responseJsonMicro.errors[0].message;
                        }
                        else
                        {
                            mensajeError = "";
                            foreach (MsDtoError error in responseJsonMicro.errors)
                            {
                                cont += 1;
                                code = error.code;
                                mensajeError += $"Error {code}: {error.message} \n";
                            }
                        }

                        throw new IndicadorGenerarException(mensajeError, mensajeError, code);

                    }
                }
                else

                    throw new IndicadorGenerarException(responsegarantiaMicro.ReasonPhrase, responsegarantiaMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri_garantia, request.identificacion, request, garantiaResponse);

            return garantiaResponse;
        }
    }
}
