using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using bg.hd.banca.pyme.domain.entities.PreCalificador;
using bg.hd.banca.pyme.domain.entities.FichaPreCalificador;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.infrastructure.utils;
using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using static bg.hd.banca.pyme.domain.entities.informacionCliente.CuentasContablesRequest;
using bg.hd.banca.pyme.domain.entities;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    public class PreCalificadorRestRepository : IPreCalificadorRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public PreCalificadorRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<PreCalificadorResponse> ConsultaHostRiesgos(PreCalificadorRequest request)
        {
            PreCalificadorResponse dataResponse = null;

            PreCalificadorRequest requestHost = new PreCalificadorRequest()
            {
                idProceso = request.idProceso,
                tipoIdentificacion = _configuration["GeneralConfig:tipoIdentificacionDescripcion"],
                identificacion = request.identificacion,
                riesgoPropuesto = request.montoSolicitado.ToString(),

            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/riesgo-actualizar";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestHost), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<PreCalificadorResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<PreCalificadorResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<PreCalificadorResponse>(responseJson.data);

                if (dataResponse.CodigoRetorno == 2)
                {
                    throw new ConsultaMesesIvaException(dataResponse.Mensaje, dataResponse.Mensaje.ToString(), 2);
                }
            }
            else
            {

                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseRating.Contains("code") && responseRating.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseRating, new JsonSerializerSettings
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

                        throw new ConsultaMesesIvaException(mensajeError, mensajeError, code);

                    }
                }
                throw new ConsultaMesesIvaException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);

            return dataResponse;
        }
        public async Task<PreCalificadorResponse> GenerarAnalisisCualitativo(PreCalificadorRequest request)
        {
            PreCalificadorResponse dataResponse = null;

            PreCalificadorRequest requestCualitativo = new PreCalificadorRequest()
            {
                idProceso = request.idProceso,
                idCliente = request.idCliente,
                fechaRevision = request.fechaRevision,

            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/analisis-cualitativo";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestCualitativo), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<PreCalificadorResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<PreCalificadorResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<PreCalificadorResponse>(responseJson.data);

                if (dataResponse.CodigoRetorno == 2)
                {
                    throw new PreCalificadorException(dataResponse.Mensaje, dataResponse.Mensaje.ToString(), 2);
                }
            }
            else
            {

                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseRating.Contains("code") && responseRating.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseRating, new JsonSerializerSettings
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

                        throw new PreCalificadorException(mensajeError, mensajeError, code);

                    }
                }
                throw new PreCalificadorException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);

            return dataResponse;
        }
        public async Task<PreCalificadorResponse> GenerarCalificacionSBS(PreCalificadorRequest request)
        {
            PreCalificadorResponse dataResponse = null;

            PreCalificadorRequest requestCualitativo = new PreCalificadorRequest()
            {
                idProceso = request.idProceso,
                idCliente = request.idCliente,

            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();            
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/calificacion-sbs";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestCualitativo), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<PreCalificadorResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<PreCalificadorResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<PreCalificadorResponse>(responseJson.data);

                if (dataResponse.CodigoRetorno == 2)
                {
                    throw new PreCalificadorException(dataResponse.Mensaje, dataResponse.Mensaje.ToString(), 2);
                }
            }
            else
            {

                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseRating.Contains("code") && responseRating.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseRating, new JsonSerializerSettings
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

                        throw new PreCalificadorException(mensajeError, mensajeError, code);

                    }
                }
                throw new PreCalificadorException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);

            return dataResponse;
        }
        public async Task<GenerarFichaPreCalificadorResponse> GenerarFichaPreCalificador(PreCalificadorRequest request)
        {
            GenerarFichaPreCalificadorResponse dataResponse = null;
            int plazomeses = 0;
            if (request.Producto == "alVencimiento")
                plazomeses = Convert.ToInt32(request.plazo == null ? 0 : request.plazo / 30);
            else
                plazomeses = Convert.ToInt32(request.plazo);

            PreCalificadorRequest requestCualitativo = new PreCalificadorRequest()
            {
                idProceso = request.idProceso,
                idCliente = request.idCliente,
                tipoIdentificacion = _configuration["GeneralConfig:tipoIdentificacionDescripcion"],
                identificacion = request.identificacion,
                montoSolicitado = request.montoSolicitado,
                destino = _configuration["InfraConfig:RatingPyme:ratingDestito"],
                plazo = plazomeses,
                tipoCalificacion = request.tipoCalificacion
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/precalificador/calcular";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestCualitativo), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<GenerarFichaPreCalificadorResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<GenerarFichaPreCalificadorResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<GenerarFichaPreCalificadorResponse>(responseJson.data);

                if (dataResponse.CodigoRetorno == 2)
                {
                    throw new PreCalificadorException(dataResponse.Mensaje, dataResponse.Mensaje.ToString(), 2);
                }
            }
            else
            {

                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseRating.Contains("code") && responseRating.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseRating, new JsonSerializerSettings
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

                        throw new PreCalificadorException(mensajeError, mensajeError, code);

                    }
                }
                throw new PreCalificadorException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);

            return dataResponse;
        }
        public async Task<PreCalificadorResponse> InformeFinalSBS(PreCalificadorRequest request)
        {
            PreCalificadorResponse dataResponse = null;

            PreCalificadorRequest requestCualitativo = new PreCalificadorRequest()
            {
                idProceso = request.idProceso,
                idCliente = request.idCliente,
                usuario = _configuration["GeneralConfig:usuarioWeb"],

            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/informe-sbs";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestCualitativo), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<PreCalificadorResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<PreCalificadorResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<PreCalificadorResponse>(responseJson.data);

                if (dataResponse.CodigoRetorno == 2)
                {
                    throw new PreCalificadorException(dataResponse.Mensaje, dataResponse.Mensaje.ToString(), 2);
                }
            }
            else
            {

                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseRating.Contains("code") && responseRating.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseRating, new JsonSerializerSettings
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

                        throw new PreCalificadorException(mensajeError, mensajeError, code);

                    }
                }
                throw new PreCalificadorException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);

            return dataResponse;
        }
        public async Task<PreCalificadorResponse> GuardarFichaPrecalificador(PreCalificadorRequest request)
        {
            PreCalificadorResponse dataResponse = null;

            PreCalificadorRequest requestCualitativo = new PreCalificadorRequest()
            {
                idProceso = request.idProceso,
                idCliente = request.idCliente,
                plazo = request.plazo,
                destino = request.destino,
                resultadoFicha = request.resultadoFicha,
                montoSolicitado = request.montoSolicitado,
                
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/precalificador";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestCualitativo), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<PreCalificadorResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<PreCalificadorResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<PreCalificadorResponse>(responseJson.data);

                if (dataResponse.CodigoRetorno == 2)
                {
                    throw new PreCalificadorException(dataResponse.Mensaje, dataResponse.Mensaje.ToString(), 2);
                }
            }
            else
            {

                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseRating.Contains("code") && responseRating.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseRating, new JsonSerializerSettings
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

                        throw new PreCalificadorException(mensajeError, mensajeError, code);

                    }
                }
                throw new PreCalificadorException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);

            return dataResponse;
        }
        public async Task<ProcDeclaracionIVAResponse> ProcesoDeclaracionIva(PreCalificadorRequest request)
        {
            ProcDeclaracionIVAResponse dataResponse = new();

            PreCalificadorRequest requestCualitativo = new PreCalificadorRequest()
            {
                idProceso = request.idProceso,
                opcion = _configuration["InfraConfig:RatingPyme:opcionPrecesarIva"],
                ordenMes = 0,//0
                anyo = 0,//
                mes =0,//0 valor 

            };


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));           
            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/precalificador/indicador-ventas";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestCualitativo), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<ProcDeclaracionIVAResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ProcDeclaracionIVAResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<ProcDeclaracionIVAResponse>(responseJson.data);

            }
            else
            {

                if ((int)responseMicro.StatusCode == 400)
                {
                    if (responseRating.Contains("code") && responseRating.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseRating, new JsonSerializerSettings
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

                        throw new PreCalificadorException(mensajeError, mensajeError, code);

                    }
                }
                throw new PreCalificadorException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);

            return dataResponse;
        }

        public async Task<CuentasContablesResponse> IngresoCuentasContables(CuentasContablesRequest request)
        {
            CuentasContablesResponse responseCuentas = new CuentasContablesResponse();
            List<string> listaSaldosIngresados = new List<string>();
            listaSaldosIngresados.Add(request.act_caja);
            listaSaldosIngresados.Add(request.act_cuentasCobrar);
            listaSaldosIngresados.Add(request.act_totalActfijos);
            listaSaldosIngresados.Add(request.act_totalInventario);
            listaSaldosIngresados.Add(request.pas_obligacionBancariaCP);
            listaSaldosIngresados.Add(request.pas_pasivosNoCtes);            
            listaSaldosIngresados.Add(request.pas_otrosPasivos);
            listaSaldosIngresados.Add(request.pas_obligacionBancariaLP);

            List<string> saldos = new List<string>();
            foreach (var item in listaSaldosIngresados)
            {
                saldos.Add(item);
            }
            
            List<CuentasContables> listacuentas = new List<CuentasContables>();
            int inc = 0;
            CuentasContables[] lista_apsett = _configuration.GetSection("ListaCuentas").Get<CuentasContables[]>();
            foreach (CuentasContables cuenta in lista_apsett)
            {
                CuentasContables obj = new CuentasContables();
                obj.idCuenta = cuenta.idCuenta;
                obj.saldo = saldos[inc];
                obj.tipo = cuenta.tipo;
                listacuentas.Add(obj);
                inc++;
            }

            request.cuentasContables = listacuentas;
            CuentasContablesMicroServRequest requestMicro = new CuentasContablesMicroServRequest()
            {
                idProceso = request.idProceso,
                idClienteRating = request.idClienteRating,
                identificacion = request.identificacion,
                tipoIdentificacion = _configuration["GeneralConfig:tipoIdentificacionDescripcion"],
                usuario = _configuration["GeneralConfig:usuarioWeb"],
                fechaRevision = request.fechaRevision,
                cuentasContables = request.cuentasContables
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            //File.WriteAllText(@"c:\temp\text.json", JsonConvert.SerializeObject(requestMicro));

            var response = new HttpResponseMessage();                    
            string uri = _configuration["InfraConfig:MicroRating:url"] + "v2/precalificador/cuentas-contables";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<CuentasContablesResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<CuentasContablesResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseCuentas = _mapper.Map<CuentasContablesResponse>(responseJson.data);

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
                        throw new InformacionClienteExeption(responseJson1.errors[0].message.ToString(), responseJson1.errors[0].message.ToString() + "(" + responseJson1.errors[0].code.ToString() + ")", 3);

                    }

                }
                else
                {
                    throw new InformacionClienteExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }

            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion.ToString(), request, responseCuentas);

            return responseCuentas;            
        }
    }

}
