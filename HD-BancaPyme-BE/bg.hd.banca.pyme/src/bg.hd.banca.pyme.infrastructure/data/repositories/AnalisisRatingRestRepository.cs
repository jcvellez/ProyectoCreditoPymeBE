using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.ClientesRatingNeo;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.application.models.dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using bg.hd.banca.pyme.application.models.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Serilog;
using Microsoft.AspNetCore.WebUtilities;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.ExcepcionAnalisis;
using bg.hd.banca.pyme.infrastructure.utils;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    public class AnalisisRatingRestRepository : IAnalisisRatingRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public AnalisisRatingRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<ClientesRatingNeoResponse> CrearAnalisis( ClientesRatingNeoRequest request)
        {
            ClientesRatingNeoResponse clienteratingResponse = new ClientesRatingNeoResponse();

           
            var clientMicro = new HttpClient();
            clientMicro.DefaultRequestHeaders.Accept.Clear();
            clientMicro.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientMicro.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            responseMicro = await clientMicro.PostAsync(uri, httpContent);
            string responseBodyMicro = await responseMicro.Content.ReadAsStringAsync();



            if (responseMicro.IsSuccessStatusCode)
            {
                MsResponse<ClientesRatingNeoResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ClientesRatingNeoResponse>>(responseBodyMicro, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                clienteratingResponse = _mapper.Map<ClientesRatingNeoResponse>(responseJson.data);

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

                        throw new AnalisisRatingCrearException(mensajeError, mensajeError, code);

                    }                    
                }
                else

                    throw new AnalisisRatingCrearException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, clienteratingResponse);

            return clienteratingResponse;

        }
        public async Task<AnyosBalanceResponse> ConsultarAnyosBalance(AnyosWebRequest request)
        {
            AnyosBalanceResponse AnyosBalanceResponse = new AnyosBalanceResponse();
            //años
            AnyosWebRequest request_anyosweb = new AnyosWebRequest();
            request_anyosweb.identificacion = request.identificacion;
            request_anyosweb.fechaRevision = request.fechaRevision;
            request_anyosweb.idProceso = request.idProceso;

            var clientMicro_anyosweb = new HttpClient();
            clientMicro_anyosweb.DefaultRequestHeaders.Accept.Clear();
            clientMicro_anyosweb.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientMicro_anyosweb.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var responseMicro_anyosweb = new HttpResponseMessage();
            string uri_anyosweb = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/balance-anyos";            
            HttpContent httpContent_anyosweb = new StringContent(JsonConvert.SerializeObject(request_anyosweb), Encoding.UTF8);
            httpContent_anyosweb.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            responseMicro_anyosweb = await clientMicro_anyosweb.PostAsync(uri_anyosweb, httpContent_anyosweb);            
            string responseBodyMicro_anyosweb = await responseMicro_anyosweb.Content.ReadAsStringAsync();

            //años fin
            if (responseMicro_anyosweb.IsSuccessStatusCode)
            {
                MsResponse<ClientesRatingNeoResponse> responseJson_anyosweb = JsonConvert.DeserializeObject<MsResponse<ClientesRatingNeoResponse>>(responseBodyMicro_anyosweb, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                //consultaDatosPersona = _mapper.Map<ConsultaPersonaResponse>(responseJson.data);
                AnyosBalanceResponse.Informar = responseJson_anyosweb.data.Informar;
                AnyosBalanceResponse.anyos = responseJson_anyosweb.data.anyos;
                 
            }
            else
            {
                if ((int)responseMicro_anyosweb.StatusCode == 400)
                {
                    if (responseBodyMicro_anyosweb.Contains("code") && responseBodyMicro_anyosweb.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBodyMicro_anyosweb, new JsonSerializerSettings
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

                        throw new InformacionClienteExeption(mensajeError, mensajeError, code);

                    }
                }
                else
                {
                    throw new InformacionClienteExeption(responseMicro_anyosweb.ReasonPhrase, responseMicro_anyosweb.RequestMessage.ToString(), 1);
                }
            }
            
            PrimitiveDataUtils.saveLogsInformation(uri_anyosweb, request.identificacion, request, AnyosBalanceResponse);

            return AnyosBalanceResponse;
        }
        public async Task<ExcepcionAnalisisResponse> GenerarExcepcionAnalisis(ExcepcionAnalisisRequest request)
        {
            ExcepcionAnalisisResponse dataResponse = new ExcepcionAnalisisResponse();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/excepcion-analisis";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);


            string responseBody = await response.Content.ReadAsStringAsync();
            MsResponse<ExcepcionAnalisisResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ExcepcionAnalisisResponse>>(responseBody, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (response.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<ExcepcionAnalisisResponse>(responseJson.data);

             
            }
            else
            {

                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
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

                        throw new GeneralException(mensajeError, mensajeError, code);

                    }
                }
               throw new GeneralException(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, dataResponse);


            return dataResponse;
        }
    }
}
