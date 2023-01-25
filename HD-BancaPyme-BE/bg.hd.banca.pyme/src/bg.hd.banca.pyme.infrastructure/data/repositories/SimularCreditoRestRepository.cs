using AutoMapper;
using Microsoft.Extensions.Configuration;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.domain.entities.SimularCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using bg.hd.banca.pyme.domain.Clases;
using Serilog;
using System.Xml;
using bg.hd.banca.pyme.application.models.exeptions;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using bg.hd.banca.pyme.domain.entities.otp;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.infrastructure.utils;
using bg.hd.banca.pyme.domain.entities;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    public class SimularCreditoRestRepository : ISimularCreditoRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public SimularCreditoRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }


        public async Task<SimularCreditoResponse> SimularCredito_CuotaMensual(SimularCreditoRequest request)
        {
            SimularCreditoResponse scresponse = new SimularCreditoResponse();
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.tipoProducto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }
            string tipoCuota = (request.tipoCuota.Equals("fija") ? "F" :
                                request.tipoCuota.Equals("variable") ? "A" :
                                throw new SimularCreditoException("tipoCuota no existe", "tipoCuota no existe", 2)
                                );

            SimularCreditoResponse simularcreditoResponse = new SimularCreditoResponse();


            SimularCreditoRequestMicroServ requestMicro = new SimularCreditoRequestMicroServ()
            {
                identificacion = request.cedula,
                monto = request.monto,
                idProducto = producto.idProducto,
                diaPago = request.diaPago,
                plazo = request.plazo,
                tipoTabla = tipoCuota,
                usuario = "",
                enviaCorreo = 0,
                idSolicitud = 0
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCreditos:url"]) + "v2/tabla-amortizacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            response = await client.PostAsync(uri, httpContent);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsResponse<SimularCreditoResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<SimularCreditoResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                simularcreditoResponse = _mapper.Map<SimularCreditoResponse>(responseJson.data);
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
                        throw new SimularCreditoException(mensajeError, mensajeError, code);

                    }
                    else
                    {
                        throw new SimularCreditoException(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                    }
                }
                else
                {
                    throw new SimularCreditoException(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, simularcreditoResponse);

            return simularcreditoResponse;

        }


        public async Task<SimularCreditoResponse> SimularCredito_AlVencimiento(SimularCreditoRequest request, double tasaNominal)
        {


            SimularCreditoResponse obj = new SimularCreditoResponse();
            if (request.plazo <= 0)
            {
                obj.mensaje = "El plazo no puede ser 0 o menor a cero";
            }
            else
            {
                obj.codigoRetorno = 0;
                obj.mensaje = "Ok-Proceso";
              
                int? capital_inicial = request.monto;
                double total = Convert.ToDouble(capital_inicial) + Convert.ToDouble(capital_inicial * request.plazo * tasaNominal) / 36000;
                total = Math.Round(total, 2);
                obj.totalPagar = total;
                obj.tasaInteres = tasaNominal;
                if (obj.tasaInteres == 0)
                {
                    throw new ConsultaTasasInteresException("Tasa de interes no puede ser 0", "Tasa de interes no puede ser 0", 2);
                }
                obj.cuota = 0;
                obj.plazo = request.plazo;
            }
            return obj;
        }

        public async Task<ConsultaTasaInteresResponse> ConsultaTasaInteres(ConsultaTasaInteresRequest request)
        {

            ConsultaTasaInteresResponse tasainteresResponse = new ConsultaTasaInteresResponse();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCreditos:url"]) + "v2/tasa";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ConsultaTasaInteresResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaTasaInteresResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                tasainteresResponse = _mapper.Map<ConsultaTasaInteresResponse>(responseJson.data);                
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

                        throw new ConsultaTasasInteresException(mensajeError, mensajeError, code);

                    }
                }
                else
                {
                    throw new ConsultaTasasInteresException(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, "", request, tasainteresResponse);
            return tasainteresResponse;
        }
    }
}