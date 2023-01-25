using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
//using bg.hd.banca.persona.domain.entities;
using bg.hd.banca.pyme.domain.entities.catalogo;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.cuentas;
using static bg.hd.banca.pyme.domain.entities.persona.ConsultarCuentaPorIdResponse;
using static bg.hd.banca.pyme.infrastructure.utils.PrimitiveDataUtils;
using bg.hd.banca.pyme.domain.entities.BancaControl;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.crmCasos;
namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class InformacionClienteRestRepository : IInformacionClienteRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public InformacionClienteRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<InformacionClienteDniResponse> InformacionCliente(string identificacion, string codDactilar)
        {
            InformacionClienteDniResponse informacionClienteDniResponse = null;

            var query = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(codDactilar))
            {
                query = new Dictionary<string, string>()
                {
                    ["numero"] = identificacion
                };
            }
            else
            {
                query = new Dictionary<string, string>()
                {
                    ["numero"] = identificacion,
                    ["dactilar"] = codDactilar
                };
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = new HttpResponseMessage();
            response = await client.GetAsync(QueryHelpers.AddQueryString(string.Format(_configuration["InfraConfig:MicroClientes:urlServiceInformacionDni"] + "v1/dni"), query));
            string responseBody = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 400)
            {
                //si dactilar esta vacio responde 200 si esta lleno retorna exception cod 2            
                if (string .IsNullOrEmpty(codDactilar))
                {
                    InformacionClienteDniResponse informacionClienteDniResponse400 = new InformacionClienteDniResponse();
                    PrimitiveDataUtils.saveLogsInformation(_configuration["InfraConfig:MicroClientes:urlServiceInformacionDni"], identificacion, "", response);
                    return informacionClienteDniResponse400;
                }
                else
                {
                    throw new InformacionClienteExeption("Cedula o Código dactilar erróneo", "Cedula o Código dactilar erróneo", 5);
                }       
            }

            MsResponse<InformacionClienteDniResponse> responseBodyJson = JsonConvert.DeserializeObject<MsResponse<InformacionClienteDniResponse>>(responseBody, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            informacionClienteDniResponse = _mapper.Map<InformacionClienteDniResponse>(responseBodyJson.data);
            if (informacionClienteDniResponse != null)
            {
                string fechaNacimiento = informacionClienteDniResponse.fechaNacimiento;
                informacionClienteDniResponse.edad = PrimitiveDataUtils.obtenerEdad(ref fechaNacimiento);
                informacionClienteDniResponse.fechaNacimiento = fechaNacimiento;
                string textoAux = string.Format(_configuration["GeneralConfig:PalabrasReservadas"]);
                informacionClienteDniResponse.persona = (IdentificaNombres)PrimitiveDataUtils.SepararNombres(informacionClienteDniResponse.nombres, PrimitiveDataUtils.typeTag.Nombre, textoAux);
            }

            PrimitiveDataUtils.saveLogsInformation(_configuration["InfraConfig:MicroClientes:urlServiceInformacionDni"], identificacion, "", response);

            return informacionClienteDniResponse;

        }
        public async Task<IdentificaUsuarioDataResponse> InformacionClienteData(string identificacion)
        {
            Log.Information("{Proceso} {Canal}", "Infomracion Clinente IN", identificacion);

            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = null;
            InformacionClienteDniResponse informacionClienteDniResponse = null;
            ConsultaPersonaResponse consultaDatosPersona = null;


            var queryInfo = new Dictionary<string, string>()
            {
                ["identificacion"] = identificacion
            };

            var clientInformacion = new HttpClient();
            clientInformacion.DefaultRequestHeaders.Accept.Clear();
            clientInformacion.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseInfo = new HttpResponseMessage();
            responseInfo = await clientInformacion.GetAsync(QueryHelpers.AddQueryString(string.Format(_configuration["InfraConfig:MicroClientes:urlServiceIdentificacionSegmento"] + "v1/informacion"), queryInfo));
            string responseBodyInfo = await responseInfo.Content.ReadAsStringAsync();

            if ((int)responseInfo.StatusCode == 400)
            {
                throw new InformacionClienteExeption("Cliente no existe", "Cliente no existe", 3);
            }

            MsResponse<IdentificaUsuarioDataResponse> responseBodyInfoJson = JsonConvert.DeserializeObject<MsResponse<IdentificaUsuarioDataResponse>>(responseBodyInfo, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            identificaUsuarioDataResponse = _mapper.Map<IdentificaUsuarioDataResponse>(responseBodyInfoJson.data);

            return identificaUsuarioDataResponse;

            PrimitiveDataUtils.saveLogsInformation(_configuration["InfraConfig:MicroClientes:urlServiceIdentificacionSegmento"], identificacion, "", identificaUsuarioDataResponse);

        }
        public async Task<ConsultaPersonaResponse> InformacionClientePersona(string identificacion)
        {
            Log.Information("{Proceso} {Canal}", "Infomracion Clinente IN", identificacion);

            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = null;
            InformacionClienteDniResponse informacionClienteDniResponse = null;
            ConsultaPersonaResponse consultaDatosPersona = null;



            #region consultaDatosPersona
            ConsultaPersonaMicroServResquest requestMicro = new ConsultaPersonaMicroServResquest()
            {
                opcion = 17,
            };

            var clientMicro = new HttpClient();
            clientMicro.DefaultRequestHeaders.Accept.Clear();
            clientMicro.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientMicro.DefaultRequestHeaders.Add("identificacion", identificacion);
            clientMicro.DefaultRequestHeaders.Add("opcion", requestMicro.opcion.ToString());
            clientMicro.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/personas/persona/id";
            responseMicro = await clientMicro.GetAsync(uri);
            string responseBodyMicro = await responseMicro.Content.ReadAsStringAsync();
            MsResponse<ConsultaPersonaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaPersonaResponse>>(responseBodyMicro, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });


            if (responseMicro.IsSuccessStatusCode)
            {
                consultaDatosPersona = _mapper.Map<ConsultaPersonaResponse>(responseJson.data);

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

                        throw new InformacionClienteExeption(mensajeError, mensajeError, code);

                    }
                }
                else
                {
                    throw new InformacionClienteExeption(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, "", consultaDatosPersona);

            #endregion
            return consultaDatosPersona;

        }
        public async Task<IdentificaNombres> ObtenerNombres(string nombres)
        {
            string textoAux = string.Format(_configuration["GeneralConfig:PalabrasReservadas"]);
            IdentificaNombres obj = (IdentificaNombres)PrimitiveDataUtils.SepararNombres(nombres, PrimitiveDataUtils.typeTag.Nombre, textoAux);
            return obj;
        }
        public async Task<ConsultaOficialResponseMicroServ> ConsultarDatosOficial(ConsultarOficialRequest parameter)
        {
            ConsultaOficialResponseMicroServ responseConsulta = new ConsultaOficialResponseMicroServ();
            responseConsulta.DataTransaccion = new Transaccion();
            responseConsulta.DataTransaccion.codigoResponse = 3; //error al consultar los datos del oficial
            bool generarException = false;
            parameter.tipoIdentificacion = Int32.Parse(string.Format(_configuration["GeneralConfig:tipoIdentificacion"]));
            parameter.canal = Int32.Parse(string.Format(_configuration["GeneralConfig:canal"]));


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("identificacion", parameter.identificacion);
            client.DefaultRequestHeaders.Add("tipoIdentificacion", parameter.tipoIdentificacion.ToString());
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();


            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/personas/persona/oficial";
            response = await client.GetAsync(uri);


            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<ConsultaOficialResponseMicroServ> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaOficialResponseMicroServ>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseConsulta = _mapper.Map<ConsultaOficialResponseMicroServ>((ConsultaOficialResponseMicroServ)responseJson.data);
                responseConsulta.DataTransaccion.codigoResponse = 0;

            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None
                    });
                    responseConsulta.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                    responseConsulta.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                }
                else
                {
                    responseConsulta.DataTransaccion.MessageResponse = response.ReasonPhrase;
                    responseConsulta.DataTransaccion.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, parameter.identificacion, parameter, responseConsulta);

            if (generarException) { throw new GestionExpedienteException(responseConsulta.DataTransaccion.MessageResponse, responseConsulta.DataTransaccion.DescriptionResponse, responseConsulta.DataTransaccion.codigoResponse); }



            return responseConsulta;
        }
        #region Grabar datos normativos
        public async Task<Transaccion> GrabarDatosNormativos(ActualizaInformacionNormativaRequest request)
        {

            Transaccion responseNormativo = new Transaccion();
            responseNormativo.codigoResponse = 11;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/personas/persona/normativa";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);


            string responseBody = await response.Content.ReadAsStringAsync();
            bool generarException = false;

            if (response.IsSuccessStatusCode)
            {
                responseNormativo.codigoResponse = 0;
                responseNormativo.MessageResponse = "Transaccion OK";
            }
            else
            {
                if (request.GenerarException) { generarException = true; }
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {
                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    responseNormativo.MessageResponse = responseJsonError.errors[0].message.ToString();
                    responseNormativo.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                }
                else
                {
                    responseNormativo.MessageResponse = response.ReasonPhrase;
                    responseNormativo.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacionCliente, request, responseNormativo);

            if (generarException) { throw new GestionExpedienteException(responseNormativo.MessageResponse, responseNormativo.DescriptionResponse, responseNormativo.codigoResponse); }

            return responseNormativo;
        }
        #endregion
        public async Task<ConsultaDatosSRIResponse> ConsultaDatosSRI(string identificacion)
        {

            ConsultaDatosSRIResponse generarResponse = new ConsultaDatosSRIResponse();

            var query = new Dictionary<string, string>()
            {
                ["dni"] = identificacion
            };


            string uri = QueryHelpers.AddQueryString(string.Format(_configuration["InfraConfig:MicroClientes:urlServiceInformacionDni"] + "v1/ruc"), query);

            rsConsultaRUCPersona responseJson = new rsConsultaRUCPersona();
            rsConsultaRUCPersona responseBody = await HTTPRequest.GetAsyncPersonalizado(uri, responseJson, responseJson);
            generarResponse.data = responseBody.data;

            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, "", generarResponse);


            return generarResponse;
        }
        public async Task<GrabarDatosPersonaResponse> GrabarDatosPersona(GrabarDatosPersonaRequest request)
        {

            GrabarDatosPersonaResponse grabarInformacionResponse = new GrabarDatosPersonaResponse();

            string textoAux = string.Format(_configuration["GeneralConfig:PalabrasReservadas"]);
            // PersonaNombre persona = PrimitiveDataUtils.ObfuscateTags(request.nombre, PrimitiveDataUtils.typeTag.Nombre, textoAux, true);

            IdentificaNombres persona = (IdentificaNombres)PrimitiveDataUtils.SepararNombres(request.nombre, PrimitiveDataUtils.typeTag.Nombre, textoAux);

            GrabarPersonaRequestMicroServ requestMicro = new GrabarPersonaRequestMicroServ()
            {
                numIdentificacion = request.identificacion,
                tipoIdentificacion = string.Format(_configuration["GeneralConfig:tipoIdentificacion"]),
                nombre = request.nombre,
                estadoCivil = request.estadoCivil,
                primerNombre = persona.primerNombre,
                segundoNombre = persona.segundoNombre,
                primerApellido = persona.primerApellido,
                segundoApellido = persona.SegundoApellido,
                situacionLaboral = request.situacionLaboral,
                idActividadEconomica = "477",
                origenIngresos = request.origenIngresos,
                idActividadCiiu = request.idActividadCiiu,
                tiempoLabora = "0",
                ingreso = "0",
                sueldoMensual = "0",
                ventaMensual = request.ventaMensual,
                fechaNacimiento = request.fechaNacimiento,
                nacionalidad = request.nacionalidad,
                idGenero = request.idGenero,
                direccionDomicilio = request.direccionDomicilio,
                direccionTrabajo = request.direccionTrabajo,
                tipoPersona = request.tipoPersona,
                fechaVenta = request.fechaVenta

            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            //(new System.Xml.Serialization.XmlSerializer(requestMicro.GetType())).Serialize(new System.IO.StreamWriter(@"c:\temp\text.xml"), requestMicro);
            //File.WriteAllText(@"c:\temp\text.json", JsonConvert.SerializeObject(requestMicro));

            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<GrabarDatosPersonaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<GrabarDatosPersonaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                grabarInformacionResponse = _mapper.Map<GrabarDatosPersonaResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, grabarInformacionResponse);

            return grabarInformacionResponse;
        }
        public async Task<ActualizaVentasClienteResponse> ActualizarVentasCliente(ActualizaVentasClienteMicroServ request)
        {
            ActualizaVentasClienteResponse generarResponse = new ActualizaVentasClienteResponse();
            ActualizaVentasClienteMicroServ requestMicro = _configuration.GetSection("GeneralConfig:ActualizaVentas:EnviarTramaHost").Get<ActualizaVentasClienteMicroServ>();
            requestMicro.UPCLFVTANUAL = request.UPCLFVTANUAL;
            requestMicro.UPCLVTAS = request.UPCLVTAS;
            

            requestMicro.CLVTAS = request.CLVTAS;
            requestMicro.CLFVTANUAL = request.CLFVTANUAL;
            requestMicro.ID = request.ID;
            requestMicro.TIPOID = request.TIPOID;

            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/core/trama/send";
            string auth = string.Format(_configuration["AzureAd:tokenName"]);
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), requestMicro);
            string responseBody = await response.Content.ReadAsStringAsync();
            Transaccion logResp = new Transaccion();
            bool generarException = HTTPRequest.ObtenerErrores(uri, response, responseBody, logResp);
            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<ActualizaVentasClienteResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<ActualizaVentasClienteResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                logResp.codigoResponse = responseJson.data.CodigoRetorno;
                logResp.MessageResponse = responseJson.data.Mensaje;

                if (responseJson.data.CodigoRetorno == 0)
                {
                    generarResponse.CodigoRetorno = responseJson.data.CodigoRetorno;
                    generarResponse.Mensaje = responseJson.data.Mensaje;

                    string idCuenta = "";
                }

            }
            //PrimitiveDataUtils.saveLogsInformation(uri, "", request, logResp);
            if (generarException)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);

            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, generarResponse);

            return generarResponse;

        }
        public Task<ConsultaActividadEconomicaResponse> ConsultaActividadEconomica(string identificacion)
        {
            throw new NotImplementedException();
        }
        public async Task<IngresarDetalleVentasResponse> IngresarDetalleVentas(IngresarDetalleVentasRequest request)
        {
            IngresarDetalleVentasResponse detalleventaResponse = new IngresarDetalleVentasResponse();
            request.Opcion = "1";
            IngresarDetalleVentasRequestMicroServ requestMicro = new IngresarDetalleVentasRequestMicroServ()
            {
                Opcion = request.Opcion,
                Identificacion = request.Identificacion,
                Productos = request.Productos,
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/informacion-ventas"; //aqui falta cambiar
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<IngresarDetalleVentasResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<IngresarDetalleVentasResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                detalleventaResponse = _mapper.Map<IngresarDetalleVentasResponse>(responseJson.data);

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

                        var mensajeError = responseJson1.errors[0].message.ToString();

                        if (responseJson1.errors[0].code == 1) { mensajeError = "Error en transaccion"; }

                        throw new InformacionClienteExeption(mensajeError, mensajeError + "(" + responseJson1.errors[0].code.ToString() + ")", 3);

                    }

                }
                else
                {
                    throw new InformacionClienteExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }

            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, detalleventaResponse);

            return detalleventaResponse;
        }
        public async Task<ConsultarDetalleVentasResponse> ConsultarDetalleVentas(string identificacion)
        {
            ConsultarDetalleVentasResponse responseDetalles = new();
            IngresarDetalleVentasRequest newRequest = new();
            IngresarDetalleVentasRequestMicroServ requestMicro = new IngresarDetalleVentasRequestMicroServ()
            {
                Opcion = "2",
                Identificacion = identificacion,
                Productos = newRequest.Productos
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/informacion-ventas"; //aqui falta cambiar
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<ConsultarDetalleVentasResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultarDetalleVentasResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseDetalles = _mapper.Map<ConsultarDetalleVentasResponse>(responseJson.data);

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
            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, "", responseDetalles);

            return responseDetalles;
        }
        public async Task<GrabaClientesProveedoresResponse> GrabaProveedorCliente(GrabaClientesProveedoresRequest request)
        {
            GrabaClientesProveedoresResponse responseClienteProveedor = new GrabaClientesProveedoresResponse();

            GrabaClientesProveedoresMicroRequest requestMicro = new GrabaClientesProveedoresMicroRequest()
            {
                Opcion = "1",
                Identificacion = request.Identificacion,
                Personas = request.Personas,
                TipoClienteProveedor = request.TipoClienteProveedor
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/proveedores-clientes"; //aqui falta cambiar
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<GrabaClientesProveedoresResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<GrabaClientesProveedoresResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseClienteProveedor = _mapper.Map<GrabaClientesProveedoresResponse>(responseJson.data);

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

                        var mensajeError = responseJson1.errors[0].message.ToString();

                        if (responseJson1.errors[0].code == 1) { mensajeError = "Error en transaccion"; }

                        throw new InformacionClienteExeption(mensajeError, mensajeError + "(" + responseJson1.errors[0].code.ToString() + ")", 3);

                    }

                }
                else
                {
                    throw new InformacionClienteExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }

            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, responseClienteProveedor);

            return responseClienteProveedor;
        }
        public async Task<ConsultaClientesProveedoresResponse> ConsultarClientesProveedores(string identificacion, int tipoClienteProveedor)
        {
            ConsultaClientesProveedoresResponse responseDetalles = new();
            IngresarDetalleVentasRequest newRequest = new();
            GrabaClientesProveedoresMicroRequest requestMicro = new GrabaClientesProveedoresMicroRequest()
            {
                Opcion = "2",
                Identificacion = identificacion,
                TipoClienteProveedor = tipoClienteProveedor
            };


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/proveedores-clientes"; //aqui falta cambiar
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<ConsultaClientesProveedoresResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaClientesProveedoresResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseDetalles = _mapper.Map<ConsultaClientesProveedoresResponse>(responseJson.data);

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

                        var mensajeError = responseJson1.errors[0].message.ToString();

                        if (responseJson1.errors[0].code == 1) { mensajeError = "Error en transaccion"; }

                        throw new InformacionClienteExeption(mensajeError, mensajeError + "(" + responseJson1.errors[0].code.ToString() + ")", 3);

                    }

                }
                else
                {
                    throw new InformacionClienteExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }

            }

            return responseDetalles;
        }
        public async Task<GrabaReferenciaBancariaResponse> GrabaReferenciasBancarias(GrabaReferenciaBancariaRequest request)
        {
            GrabaReferenciaBancariaResponse responseReferencia = new GrabaReferenciaBancariaResponse();

            GrabaReferenciaBancariaRequestMicroServ requestMicro = new GrabaReferenciaBancariaRequestMicroServ()
            {
                Opcion = "1",
                Identificacion = request.Identificacion,
                Referencias = request.Referencia
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/referencias-bancarias";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<GrabaReferenciaBancariaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<GrabaReferenciaBancariaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseReferencia = _mapper.Map<GrabaReferenciaBancariaResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, responseReferencia);

            return responseReferencia;

        }
        public async Task<ConsultaReferenciaBancariaResponse> ConsultaReferenciasBancarias(string identificacion)
        {
            ConsultaReferenciaBancariaResponse responseReferencia = new ConsultaReferenciaBancariaResponse();

            ConsultaReferenciaBancariaRequestMicroServ requestMicro = new ConsultaReferenciaBancariaRequestMicroServ()
            {
                Opcion = "2",
                Identificacion = identificacion
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/referencias-bancarias";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<ConsultaReferenciaBancariaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaReferenciaBancariaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseReferencia = _mapper.Map<ConsultaReferenciaBancariaResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, identificacion, responseReferencia);

            return responseReferencia;
        }
        public async Task<ConsultarCuentaPorIdResponse> ConsultarCuentaPorId(string identificacion)
        {
            List<CUENTAS_OUT>? lista = new List<CUENTAS_OUT>();
            List<CUENTAS_CLIENTE>? listanueva = new List<CUENTAS_CLIENTE>();

            ConsultaCuentasRequest consultarequest = _configuration.GetSection("GeneralConfig:Cuentas:EnviarTramaHost").Get<ConsultaCuentasRequest>();
            consultarequest.ITIPOID = _configuration["GeneralConfig:tipoIdentificacionDescripcion"];
            consultarequest.IID = identificacion;

            ConsultarCuentaPorIdResponse generarResponse = new ConsultarCuentaPorIdResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;

            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/core/trama/send";
            string auth = string.Format(_configuration["AzureAd:tokenName"]);
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), consultarequest);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(uri, response, responseBody, logResp);
            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<ConsultaCuentasResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<ConsultaCuentasResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                logResp.codigoResponse = responseJson.data.CodigoRetorno;
                logResp.MessageResponse = responseJson.data.Mensaje;

                if (responseJson.data.CodigoRetorno == 0)
                {
                    List<string> Arreglo_TipoCuenta = new List<string>();
                    var Cuentas = responseJson.data.INFORMACION.CUENTAS_OUT.Where(elem => elem.ESTADO_CUENTA.Trim().Equals("0") && elem.RELACION_CLIENTE.Trim().Equals("TIT") && (elem.TIPO_CUENTA.Trim().Equals("CA") || elem.TIPO_CUENTA.Trim().Equals("CC")))
                                .Select(elem => elem).ToList();
                    Cuentas.ForEach(elem => Arreglo_TipoCuenta.Add(elem.TIPO_CUENTA));
                    generarResponse.CodigoRetorno = responseJson.data.CodigoRetorno;
                    generarResponse.Mensaje = responseJson.data.Mensaje;


                    if (Cuentas == null)
                    {
                        generarResponse.crearCuenta = true;
                    }
                    else
                    {
                        generarResponse.crearCuenta = false;

                        foreach (var item in Cuentas)
                        {
                            switch (item.TIPO_CUENTA)
                            {
                                case "CC":
                                    //Corriente
                                    item.TIPO_CUENTA = "4594";
                                    break;
                                case "CA":
                                    //Ahorros
                                    item.TIPO_CUENTA = "4593";
                                    break;
                            }
                            CUENTAS_CLIENTE obj = new CUENTAS_CLIENTE();
                            obj.Tipo_Cuenta = item.TIPO_CUENTA;
                            obj.Numero_Cuenta = item.NUM_CUENTA;
                            string ofuscate = PrimitiveDataUtils.ObfuscateType(obj.Numero_Cuenta, typeTag.numeroCuenta);
                            obj.Numero_Cuenta_ofuscate = ofuscate;
                            listanueva.Add(obj);
                        }

                        generarResponse.CuentasClienteActivas = listanueva;
                    }
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, identificacion, logResp);
            if (generarException)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);

            }

            return generarResponse;
        }
        public async Task<GrabaCertificacionAmbientalResponse> GrabaCertificacionesAmbientales(GrabaCertificacionAmbientalRequest request)
        {
            GrabaCertificacionAmbientalResponse responseCertificaciones = new GrabaCertificacionAmbientalResponse();

            GrabaCertificacionAmbientalMicroRequest requestMicro = new GrabaCertificacionAmbientalMicroRequest()
            {
                Opcion = "1",
                Identificacion = request.Identificacion,
                Certificaciones = request.certificaciones
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/certificaciones-ambientales";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<GrabaCertificacionAmbientalResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<GrabaCertificacionAmbientalResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseCertificaciones = _mapper.Map<GrabaCertificacionAmbientalResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, responseCertificaciones);
            return responseCertificaciones;
        }
        public async Task<ConsultarDetalleCertificadosResponse> ConsultarCertificacionesAmbientales(string identificacion)
        {
            ConsultarDetalleCertificadosResponse responseDetalles = new();
            GrabaCertificacionAmbientalMicroRequest requestMicro = new GrabaCertificacionAmbientalMicroRequest()
            {
                Opcion = "2",
                Identificacion = identificacion
            };


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/certificaciones-ambientales";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<ConsultarDetalleCertificadosResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultarDetalleCertificadosResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseDetalles = _mapper.Map<ConsultarDetalleCertificadosResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, "", responseDetalles);

            return responseDetalles;
        }
        public async Task<ConsultaInstalacionesSegurosResponse> ConsultaInstalacionSeguros(string identificacion)
        {
            ConsultaInstalacionesSegurosResponse responseMicro = new();
            IngresarDetalleVentasRequest newRequest = new();
            ConsultaInstalacionesSegurosMicroRequest requestMicro = new ConsultaInstalacionesSegurosMicroRequest()
            {
                Opcion = "2",
                Identificacion = identificacion,

            };


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/datos-negocios";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<ConsultaInstalacionesSegurosResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaInstalacionesSegurosResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseMicro = _mapper.Map<ConsultaInstalacionesSegurosResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, "", responseMicro);


            return responseMicro;
        }
        public async Task<GrabaInstalacionesSegurosResponse> GrabaInstalacionSeguros(GrabaInstalacionesSegurosRequest request)
        {
            GrabaInstalacionesSegurosResponse responseMicro = new();
            IngresarDetalleVentasRequest newRequest = new();
            GrabaInstalacionesSegurosMicroRequest requestMicro = new GrabaInstalacionesSegurosMicroRequest()
            {
                Identificacion = request.Identificacion,
                Opcion = "1",
                Propio = request.Propio,
                CompaniaSeguro = request.CompaniaAseguradora,
                NumeroEmpleados = request.NumeroEmpleados
            };


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/datos-negocios";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                MsResponse<GrabaInstalacionesSegurosResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<GrabaInstalacionesSegurosResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseMicro = _mapper.Map<GrabaInstalacionesSegurosResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, responseMicro);


            return responseMicro;
        }
        public async Task<BancaControlResponse> ConsultarEstadoTarjetaVirtual(BancaControlRequest request, bool controlarException)
        {
            BancaControlResponse generarResponse = new BancaControlResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";

            BancaControlRequest bancaControlConfig = _configuration.GetSection("BancaControlConfig").Get<BancaControlRequest>();
            request.canal = bancaControlConfig.canal;
            request.terminal = bancaControlConfig.terminal;


            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/EntrustInterface/tarjetaVirtual/serieEstadoSolicitud";
            string auth = string.Format(_configuration["AzureAd:tokenName"]);
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(uri, response, responseBody, logResp);
            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<BancaControlResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<BancaControlResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                generarResponse = responseJson.data;
                logResp.codigoResponse = generarResponse.CodigoRetorno;
                logResp.MessageResponse = generarResponse.Mensaje;
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, logResp);
            if (controlarException)
            {
                if (generarException)
                {
                    throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, generarResponse);


            return generarResponse;
        }
        public async Task<ConsultaDatosRUCResponse> ConsultaDatosRUC(string identificacion)
        {
            ConsultaDatosRUCResponse generarResponse = new ConsultaDatosRUCResponse();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("identificacion", identificacion);

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroPersonas:url"]) + "v1/ruc";
            response = await client.GetAsync(uri);
            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {

                ConsultaDatosRUCResponse responseJson = JsonConvert.DeserializeObject<ConsultaDatosRUCResponse>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                generarResponse = _mapper.Map<ConsultaDatosRUCResponse>(responseJson);
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
                    throw new InformacionClienteExeption("Error Api - ", "Error Consulta Datos Ruc", 1);
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, "", generarResponse);
            return generarResponse;
        }
        public async Task<ActualizaPatrimonioResponse> ActualizarDataCrm(ActualizaPatrimonioRequest request)
        {
            ActualizaPatrimonioResponse generarResponse = new ActualizaPatrimonioResponse();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/patrimonio";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {


                MsResponse<ActualizaPatrimonioResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ActualizaPatrimonioResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                generarResponse = _mapper.Map<ActualizaPatrimonioResponse>(responseJson.data);
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
                    throw new InformacionClienteExeption("Error Api - ", "Error Consulta Datos Ruc", 1);
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.numIdentificacion, request, generarResponse);
            return generarResponse;
        }

        public async Task<GestionResidenciaFiscalResponse> GestionarResidenciaFiscal(GestionResidenciaFiscalRequest request)
        {
            string nombreMetodo = "GestionarResidenciaFiscal";
            Transaccion logResp = new Transaccion();
            GestionResidenciaFiscalResponse generarResponse = new GestionResidenciaFiscalResponse();
            bool generarException = false;
            string uri = "";

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                generarException = true;
                logResp.codigoResponse = 2;
                logResp.DescriptionResponse = "Producto ingresado no es correcto";
                logResp.MessageResponse = "Producto ingresado no es correcto";
            }
            else
            {
                request.idTipoIdentificacion = string.Format(_configuration["GeneralConfig:tipoIdentificacion"]);
                uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/residencia-fiscal";
                string auth = _configuration["AzureAd:tokenName"];
                HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
                string responseBody = await response.Content.ReadAsStringAsync();
                generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);
                if (response.IsSuccessStatusCode)
                {
                    MsDtoResponse<GestionResidenciaFiscalResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<GestionResidenciaFiscalResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    logResp.codigoResponse = responseJson.data.CodigoRetorno;
                    logResp.MessageResponse = responseJson.data.Mensaje;
                    if (responseJson.data.CodigoRetorno == 0)
                    {
                        generarResponse = responseJson.data;
                    }
                }
                else
                {
                    generarResponse.CodigoRetorno = logResp.codigoResponse;
                    generarResponse.Mensaje = logResp.DescriptionResponse;
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.numIdentificacion, request, logResp);
            if (generarException && request.controlarExcepcion)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);

            }
            return generarResponse;
        }

        public async Task<ConsultaResidenciaFiscalResponse> ConsultaResidenciaFiscal(ConsultaResidenciaFiscalRequest request)
        {            
            string nombreMetodo = "ConsultaResidenciaFiscal";
            Transaccion logResp = new Transaccion();
            ConsultaResidenciaFiscalResponse responseConsulta = new ConsultaResidenciaFiscalResponse();
            bool generarException = false;
            string uri = "";
            #region valores de trama  a grabar
            List<string> atributosIncluidos = new List<string>()
            {
                "opcion",
                "identidad"
            };
            #endregion           

            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/personas/persona/residencia-fiscal/id";
            string auth = _configuration["AzureAd:tokenName"];
            #region Declaracion de Headers Request
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("opcion", request.opcion);
            headers.Add("identidad", request.identidad);
            headers.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            #endregion            
            HttpResponseMessage response = await HTTPRequest.GetAsync(uri, headers);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ConsultaResidenciaFiscalResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaResidenciaFiscalResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseConsulta = responseJson.data;
                logResp.codigoResponse = responseJson.data.CodigoRetorno;
                logResp.MessageResponse = $"{nombreMetodo} -  exitosa";
                logResp.DescriptionResponse = responseJson.data.Mensaje;
            }
            else
            {
                responseConsulta.CodigoRetorno = logResp.codigoResponse;
                responseConsulta.Mensaje = logResp.MessageResponse + " -  " + logResp.DescriptionResponse;
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.identidad, request, logResp);
            if (generarException && request.controlExcepcion)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);
            }
            return responseConsulta;
        }

        public async Task<DatosRCResponse> ConsultaRCDatos(string identificacion)
        {
            DatosRCResponse datosRCResponse = new DatosRCResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = string.Format(_configuration["InfraConfig:MicroPersonas:urlConsultaRC"], identificacion);
            HttpResponseMessage response = await HTTPRequest.GetAsync(uri);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);
            if (response.IsSuccessStatusCode)
            {
                MsResponse<DatosRCResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<DatosRCResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                datosRCResponse = _mapper.Map<DatosRCResponse>(responseJson.data);
                //datosRCResponse.logResp.codigoResponse = 0;
                //datosRCResponse.logResp.DescriptionResponse = "Transaccion ok";
                logResp.codigoResponse = 0;
                logResp.MessageResponse = "Transaccion ok";
                logResp.DescriptionResponse = "Transaccion ok - (metodo: ConsultaRCDatos )";
                if (datosRCResponse != null)
                {
                    string fechaNacimiento = datosRCResponse.fechaNacimiento;
                    datosRCResponse.edad = PrimitiveDataUtils.obtenerEdad(ref fechaNacimiento);
                    //datosRCResponse.fechaNacimientoConvert = fechaNacimiento;
                    string textoAux = string.Format(_configuration["GeneralConfig:PalabrasReservadas"]);
                    //datosRCResponse.Persona = PrimitiveDataUtils.ObfuscateTags(datosRCResponse.nombres, PrimitiveDataUtils.typeTag.Nombre, textoAux, false);
                }
            }
            else
            {
                //datosRCResponse.logResp.codigoResponse = 9;
                //datosRCResponse.logResp.DescriptionResponse = "Error en Consulta de Registro Civil";
            }
            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, null, identificacion, null, logResp);
            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, identificacion, logResp);
            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }

            return datosRCResponse;
        }

        public async Task<ActualizaRFCrmResponse> ActualizaResidenciaFiscalCrm(ActualizaRFCrmRequest request)
        {
            string nombreMetodo = "ActualizaResidenciaFiscalCrm";
            Transaccion logResp = new Transaccion();
            ActualizaRFCrmResponse responseActualiza = new ActualizaRFCrmResponse();
            bool generarException = false;
            string uri = "";

            uri = _configuration["InfraConfig:MicroCreditos:url"] + "v2/crmcasos/normativa/residencia-fiscal";
            string auth = _configuration["AzureAd:tokenName"];

            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ActualizaRFCrmResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ActualizaRFCrmResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseActualiza = responseJson.data;
                logResp.codigoResponse = responseJson.data.Codigo;
                logResp.MessageResponse = $"{nombreMetodo} -  exitosa";
                logResp.DescriptionResponse = responseJson.data.Mensaje;
            }
            else
            {
                responseActualiza.Codigo = logResp.codigoResponse;
                responseActualiza.Mensaje = logResp.MessageResponse + " -  " + logResp.DescriptionResponse;
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, logResp);
            if (generarException && request.controlExcepcion)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);
            }
            return responseActualiza;
        }

        public async Task<List<ConsultaContratoPorCanalResponse>> ConsultaContratoPorCanal(string identificacion, string canal)
        {           
            List<ConsultaContratoPorCanalResponse> contratoResponse = new List<ConsultaContratoPorCanalResponse>();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = _configuration["InfraConfig:MicroPersonas:urlContratos"] + $"/v1/canal?identificacion={identificacion}&canal={canal}";
            HttpResponseMessage response = await HTTPRequest.GetAsync(uri);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);
            if (response.IsSuccessStatusCode)
            {
                MsResponse<List<ConsultaContratoPorCanalResponse>> responseJson = JsonConvert.DeserializeObject<MsResponse<List<ConsultaContratoPorCanalResponse>>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                contratoResponse = _mapper.Map<List<ConsultaContratoPorCanalResponse>>(responseJson.data);
                
                logResp.codigoResponse = 0;
                logResp.MessageResponse = "Transaccion ok";
                logResp.DescriptionResponse = "Transaccion ok - (metodo: ConsultaRCDatos )";                
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
                        throw new InformacionClienteExeption(responseJson.message, responseJson.errors[0].message,2);
                    }
                }
            }            
            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, identificacion, logResp);
            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }

            return contratoResponse;
        }

        public async Task<RegistroContratosResponse> RegistroContratos(RegistroContratosRequest request)
        {            
            RegistroContratosResponse registroResponse = new RegistroContratosResponse();

            var client = new HttpClient();            
            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroPersonas:urlContratos"] + "/v1/concentimientos";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, httpContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                MsResponse<RegistroContratosResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<RegistroContratosResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                registroResponse.CodigoRetorno = responseJson.data.codigoRetorno;
                registroResponse.Mensaje = responseJson.data.mensajeRetorno;                
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
                        throw new InformacionClienteExeption(responseJson1.message, responseJson1.errors[0].message, 2);
                    }
                }
                else
                {
                    throw new InformacionClienteExeption("Error Api - ", "Error Consulta Datos Ruc", 1);
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, registroResponse);
            return registroResponse;
        }
    }
}
