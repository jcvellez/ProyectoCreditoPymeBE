using AutoMapper;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.catalogo;
using System.Net.Http.Headers;
using bg.hd.banca.pyme.domain.entities.expediente.detalle;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class GestionExpedienteRestRepository : IGestionExpedienteRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IInformacionClienteRestRepository _informacionClienteRepository;
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;
        private readonly IAuthenticationServiceRepository _authentication;
        public GestionExpedienteRestRepository(IConfiguration configuration, IMapper mapper, IInformacionClienteRestRepository informacionClienteRepository, IConsultarCatalogoRestRepository consultarCatalogoRestRepository, IAuthenticationServiceRepository Authentication)
        {
            _configuration = configuration;
            _mapper = mapper;
            _informacionClienteRepository = informacionClienteRepository;
            _consultarCatalogoRestRepository = consultarCatalogoRestRepository;
            _authentication = Authentication;
        }
        public async Task<CrearExpedienteResponse> IngresoExpediente(IngresoExpedienteRequest request)
        {
            var idProducto = "";

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.descriptionProducto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }

            idProducto = producto.idProducto;

            CrearExpedienteResponse responseConsulta = new CrearExpedienteResponse();

            request.idProducto = idProducto;
            request.descriptionProducto = string.Empty;
            request.idCanal = string.Format(_configuration["GeneralConfig:canal"]);
            responseConsulta.DataTransaccion = new Transaccion();
            responseConsulta.DataTransaccion.codigoResponse = 6; //error al ingreso del expediente
            bool generarException = false;


            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/expedientes/expediente";
            HttpResponseMessage response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<CrearExpedienteResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<CrearExpedienteResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseConsulta = _mapper.Map<CrearExpedienteResponse>((CrearExpedienteResponse)responseJson.data);
                responseConsulta.DataTransaccion.codigoResponse = 0;
                responseConsulta.DataTransaccion.MessageResponse = "Expediente Creado";
                responseConsulta.DataTransaccion.DescriptionResponse = "Expediente N.- " + responseConsulta.idExpediente;
            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    responseConsulta.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                    responseConsulta.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                    if (responseJsonError.errors[0].code == 1 || responseJsonError.errors[0].code == 6)
                    {
                        responseConsulta.DataTransaccion.MessageResponse = "Error en transaccion";
                        responseConsulta.DataTransaccion.DescriptionResponse = "Error en transaccion";
                    }

                }
                else
                {
                    responseConsulta.DataTransaccion.MessageResponse = response.ReasonPhrase;
                    responseConsulta.DataTransaccion.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, responseConsulta.DataTransaccion);

            if (generarException) { throw new GestionExpedienteException(responseConsulta.DataTransaccion.MessageResponse, responseConsulta.DataTransaccion.DescriptionResponse, responseConsulta.DataTransaccion.codigoResponse); }



            return responseConsulta;
        }

        public async Task<Solicitud> ConsultarSolicitud(SolicitudRequest request)
        {
            Solicitud responseSolicitud = new Solicitud();
            responseSolicitud.DataTransaccion = new Transaccion();
            responseSolicitud.DataTransaccion.codigoResponse = 3;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("solicitudId", request.IdSolicitud);
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();

            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/expedientes/solicitud/id";
            response = await client.GetAsync(uri);


            string responseBody = await response.Content.ReadAsStringAsync();
            bool generarException = false;

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ModificarSolicitudRequestMicroServ> responseJson = JsonConvert.DeserializeObject<MsResponse<ModificarSolicitudRequestMicroServ>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseSolicitud.DataTransaccion.codigoResponse = 0;
                responseSolicitud.DataSolicitud = new ModificarSolicitudRequestMicroServ();
                responseSolicitud.DataSolicitud = _mapper.Map<ModificarSolicitudRequestMicroServ>((ModificarSolicitudRequestMicroServ)responseJson.data);
            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    responseSolicitud.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                    responseSolicitud.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                }
                else
                {
                    responseSolicitud.DataTransaccion.MessageResponse = response.ReasonPhrase;
                    responseSolicitud.DataTransaccion.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, responseSolicitud.DataTransaccion);

            if (generarException) { throw new GestionExpedienteException(responseSolicitud.DataTransaccion.MessageResponse, responseSolicitud.DataTransaccion.DescriptionResponse, responseSolicitud.DataTransaccion.codigoResponse); }

            return responseSolicitud;
        }

        public async Task<ValidarPoliticasResponse> ValidarPoliticas(ValidarPoliticasRequest request)
        {
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.IdProducto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }

            ValidarPoliticasResponse responsePoliticas = new ValidarPoliticasResponse();


            request.IdProducto = producto.idProducto;

            responsePoliticas.DataTransaccion = new Transaccion();
            responsePoliticas.DataTransaccion.codigoResponse = 8;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/expedientes/politica/validacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);


            string responseBody = await response.Content.ReadAsStringAsync();
            bool generarException = false;

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ValidarPoliticasResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ValidarPoliticasResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responsePoliticas.DataTransaccion.codigoResponse = 0;
                responsePoliticas.DataTransaccion.MessageResponse = "Validacion OK";
                responsePoliticas = _mapper.Map<ValidarPoliticasResponse>((ValidarPoliticasResponse)responseJson.data);
            }
            else
            {
                if (request.GenerarException) { generarException = true; }
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    responsePoliticas.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                    responsePoliticas.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                }
                else
                {
                    responsePoliticas.DataTransaccion.MessageResponse = response.ReasonPhrase;
                    responsePoliticas.DataTransaccion.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.IdentificacionTitular, request, responsePoliticas.DataTransaccion);

            if (generarException) { throw new GestionExpedienteException(responsePoliticas.DataTransaccion.MessageResponse, responsePoliticas.DataTransaccion.DescriptionResponse, responsePoliticas.DataTransaccion.codigoResponse); }

            return responsePoliticas;
        }

        public async Task<ConsultarConfiguracionResponse> ConsultarConfiguracion(ConsultarConfiguracionRequest request)
        {

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.idProducto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }

            ConsultarConfiguracionResponse responseExpediente = new ConsultarConfiguracionResponse();


            request.idProducto = producto.idProducto;

            responseExpediente.DataTransaccion = new Transaccion();
            responseExpediente.DataTransaccion.codigoResponse = 9;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/expedientes/expediente/configuracion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);


            string responseBody = await response.Content.ReadAsStringAsync();
            bool generarException = false;

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ConsultarConfiguracionResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultarConfiguracionResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseExpediente.DataTransaccion.codigoResponse = 0;
                responseExpediente.DataTransaccion.MessageResponse = "Consulta éxitosa";
                responseExpediente = _mapper.Map<ConsultarConfiguracionResponse>((ConsultarConfiguracionResponse)responseJson.data);
            }
            else
            {
                if (request.GenerarException) { generarException = true; }
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    responseExpediente.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                    responseExpediente.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                }
                else
                {
                    responseExpediente.DataTransaccion.MessageResponse = response.ReasonPhrase;
                    responseExpediente.DataTransaccion.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, responseExpediente.DataTransaccion);

            if (generarException) { throw new GestionExpedienteException(responseExpediente.DataTransaccion.MessageResponse, responseExpediente.DataTransaccion.DescriptionResponse, responseExpediente.DataTransaccion.codigoResponse); }

            return responseExpediente;
        }

        #region Actualizar expediente
        public async Task<Transaccion> ModificarExpediente(ModificarExpedienteRequest request)
        {
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.idProducto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }
            Transaccion responseExpediente = new Transaccion();

            request.idProducto = producto.idProducto;

            responseExpediente.codigoResponse = 10;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/expedientes/expediente/actualizacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);


            string responseBody = await response.Content.ReadAsStringAsync();
            bool generarException = false;

            if (response.IsSuccessStatusCode)
            {
                responseExpediente.codigoResponse = 0;
                responseExpediente.MessageResponse = "Actualización éxitosa";
            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    responseExpediente.MessageResponse = responseJsonError.errors[0].message.ToString();
                    responseExpediente.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                    if (responseJsonError.errors[0].code == 400)
                    {
                        responseExpediente.MessageResponse = "Error en transaccion";
                        responseExpediente.DescriptionResponse = "Error en transaccion";
                    }

                }
                else
                {
                    responseExpediente.MessageResponse = response.ReasonPhrase;
                    responseExpediente.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, responseExpediente);

            if (generarException) { throw new GestionExpedienteException(responseExpediente.MessageResponse, responseExpediente.DescriptionResponse, responseExpediente.codigoResponse); }

            return responseExpediente;
        }

        public async Task<ActualizarExpedienteResponse> ActualizarExpediente(ActualizarExpedienteRequest request)
        {
            ///////IngresoProductoActivo 
            ActualizarExpedienteResponse dataResponse = new ActualizarExpedienteResponse();

            /**** idPeriodicidad ***/
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.idProducto, _configuration);
            /*****fin idPeriodicidad*****************/

            /**** ParametrosGenerales *****************/
            ParametrosGeneralesRequest parametrosRequest = new();
            parametrosRequest.opcion = 2;
            parametrosRequest.idProducto = (producto.nombreProducto == request.idProducto) ? producto.idProducto : null;
            parametrosRequest.idCodigo = "0";
            ParametrosGeneralesResponse parametroResponse = new ParametrosGeneralesResponse();
            parametroResponse = await ParametrosGenerales(parametrosRequest);

            var garantiaCredito = "";
            var propositoCredito = "";
            foreach (var item in parametroResponse.DataSet.Table)
            {
                if (item.nombreFisico.Equals("idSubtipoGarantia")) { request.idSubtipoGarantia = item.valorCampo; }
                if (item.nombreFisico.Equals("idTipoGarantia")) { garantiaCredito = item.valorCampo; }
                if (item.nombreFisico.Equals("idCompaniaAseguradora")) { request.idCompaniaAseguradora = item.valorCampo; }
                if (item.nombreFisico.Equals("propositoCredito")) { propositoCredito = item.valorCampo; }
            }
            /**** fin ParametrosGenerales *****************/
            /*-----------------*/
            if (producto.nombreProducto == "cuotaMensual")
            {
                request.idProducto = producto.idProducto;
            }
            else if (producto.nombreProducto == "alVencimiento")
            {
                request.idProducto = producto.idProducto;
                request.idTipoAmortizacion = null;
                request.diaPago = null;
            }



            //if (request.idTipoCuentaCredito != null)
            //{
            //    TipoCuentaCredito = request.idTipoCuentaCredito;
            //    NumCuentaCredito = request.idNumCuentaCredito;
            //    TipoCuentaDebito = request.idTipoCuentaDebito;
            //    NumCuentaDebito = request.idNumCuentaDebito;

            //    //requestCualitativo.bancoCtaCreditoDebito = _configuration["GeneralConfig:bancoCtaCreditoDebito"];
            //}

            //request.idProducto = (producto.nombreProducto == "cuotaMensual") ? producto.idProducto
            //                        : (producto.nombreProducto == "alVencimiento") ? producto.idProducto
            //                        : throw new ActualizarExpedienteException("idProducto no existe", "idProducto no existe", 2);
            /*-----------------*/
            var Ctctipo = request.Ctctipo != null ? request.Ctctipo : null;
            var CtcNumero = request.CtcNumero != null ? request.CtcNumero : null;

            ActualizarExpedienteMicroRequest requestExpMicro = new ActualizarExpedienteMicroRequest()
            {
                identificacion = request.identificacion,
                idExpediente = request.idExpediente,
                idPersona = request.idPersona,
                idProducto = request.idProducto,
                usuarioEtapa = request.usuarioEtapa,
                nombreEnTarjeta = request.nombreEnTarjeta,
                montoFinanciar = request.montoFinanciar,
                tasaInteresProducto = request.tasaInteresProducto,
                tasaInteresSolicada = request.tasaInteresProducto,
                idPeriodicidad = producto.tipoPeriodicidad,
                plazo = request.plazo,
                diaPago = request.diaPago,
                idTipoAmortizacion = request.idTipoAmortizacion,
                idGarantiaCredito = garantiaCredito,
                idSubtipoGarantia = request.idSubtipoGarantia,
                idCompaniaAseguradora = request.idCompaniaAseguradora,
                propositoCredito = propositoCredito,
                idPaisDestinoFondos = request.idPaisDestinoFondos,
                idProvinciaDestinoFondos = request.idProvinciaDestinoFondos,
                idCiudadDestinoFondos = request.idCiudadDestinoFondos,
                idParroquiaDestinoFondos = request.idParroquiaDestinoFondos,
                idDireccion = request.idDireccion,
                idModulo = request.idModulo,
                idFormulario = request.idFormulario,
                strUsuario = request.strUsuario,
                bancoCtaCreditoDebito = request.bancoCtaCreditoDebito,
                nombreLibretaChequera = request.nombreEnLibretaChequera,
                idTipoCuentaCredito = Ctctipo,
                idNumCuentaCredito = CtcNumero,
                idTipoCuentaDebito = request.TipoCuentaDebito,
                idNumCuentaDebito = request.NumCuentaDebito,
                idBancoCredito = _configuration["GeneralConfig:bancoCtaCreditoDebito"],
                idBancoDebito = _configuration["GeneralConfig:bancoCtaCreditoDebito"]

            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/expedientes/expediente/activo";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestExpMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<ActualizarExpedienteResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ActualizarExpedienteResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<ActualizarExpedienteResponse>(responseJson.data);

                if (dataResponse.CodigoRetorno == 2)
                {
                    throw new ActualizarExpedienteException(dataResponse.Mensaje, dataResponse.Mensaje.ToString(), 2);
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

                        throw new ActualizarExpedienteException(mensajeError, mensajeError, code);

                    }
                }
                throw new ActualizarExpedienteException(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            dataResponse.Producto = producto;

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, dataResponse);


            return dataResponse;
        }

        public async Task<ParametrosGeneralesResponse> ParametrosGenerales(ParametrosGeneralesRequest requestparametros)
        {
            ParametrosGeneralesResponse consultaDatosPersona = null;
            /**** idPeriodicidad ***/
            Producto producto = PrimitiveDataUtils.obtenerProducto(requestparametros.idProducto, _configuration);
            /*****fin idPeriodicidad*****************/


            var clientMicro = new HttpClient();
            clientMicro.DefaultRequestHeaders.Accept.Clear();
            clientMicro.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientMicro.DefaultRequestHeaders.Add("idProducto", requestparametros.idProducto.ToString());
            clientMicro.DefaultRequestHeaders.Add("opcion", requestparametros.opcion.ToString());
            clientMicro.DefaultRequestHeaders.Add("idCodigo", requestparametros.idCodigo.ToString());
            clientMicro.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/productos/parametros-generales/id";
            responseMicro = await clientMicro.GetAsync(uri);
            string responseBodyMicro = await responseMicro.Content.ReadAsStringAsync();
            MsResponse<ParametrosGeneralesResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ParametrosGeneralesResponse>>(responseBodyMicro, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });


            if (responseMicro.IsSuccessStatusCode)
            {
                consultaDatosPersona = _mapper.Map<ParametrosGeneralesResponse>(responseJson.data);

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
            #endregion
            PrimitiveDataUtils.saveLogsInformation(uri, "", requestparametros, consultaDatosPersona);

            return consultaDatosPersona;
        }

        #region "Método Crear Solicitud"
        public async Task<CrearSolicitudResponse> CrearSolicitud(CrearSolicitudRequest request)
        {

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.Producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 3);
            }

            CrearSolicitudResponse crearSolicitudResponse = new CrearSolicitudResponse();

            CrearSolicitudRequestMicroServ requestMicro = new CrearSolicitudRequestMicroServ()
            {
                Opcion = "1",//string.Format(_configuration["InfraConfig:MicroExpediente:crear"]), 
                IdCanal = string.Format(_configuration["GeneralConfig:canal"]),
                IdProducto = producto.idProducto,
                IdTipoIdentificacion = string.Format(_configuration["GeneralConfig:tipoIdentificacion"]),
                NumIdentificacion = request.Identificacion,
                Nombre = request.Nombre != null ? request.Nombre.Replace("'", "") : request.Nombre,
                Usuario = string.Format(_configuration["GeneralConfig:usuario"]),
                AutorizaConsultaBuro = "S",//string.Format(_configuration["InfraConfig:MicroExpediente:autoriza"]), 
                CorreoElectronico = "",
                IdSolicitud = "",
                IdExpediente = ""
            };
          
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/expedientes/solicitud";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                MsResponse<CrearSolicitudResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<CrearSolicitudResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                crearSolicitudResponse = _mapper.Map<CrearSolicitudResponse>(responseJson.data);
                Log.Information("{Proceso} {Canal}", "CrearSolicitud OUT", crearSolicitudResponse.IdSolicitud);
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
                        throw new GestionExpedienteException(responseJson1.errors[0].message.ToString(), responseJson1.errors[0].message.ToString() + "(" + responseJson1.errors[0].code.ToString() + ")", 3);

                    }

                }
                else
                {
                    throw new GestionExpedienteException(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, crearSolicitudResponse);

            return crearSolicitudResponse;


        }
        #endregion
        public async Task<ModificarSolicitudResponse> ModificarSolicitud(ModificarSolicitudRequest request)
        {

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.Producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }

            ModificarSolicitudResponse modificarSolicitudResponse = new ModificarSolicitudResponse();
            modificarSolicitudResponse.DataTransaccion.codigoResponse = 7;


            string MoToCred = request.MontoTotalCredito;

            bool generarExceptionModificar = false;
            string TipoTasaAux = string.Empty;
            if (request.TipoTabla is not null)
            {

                switch (request.TipoTabla)
                {
                    case "A":
                        TipoTasaAux = string.Format(_configuration["GeneralConfig:tablaAmortizacion:alemana"]);
                        break;
                    case "F":
                        TipoTasaAux = string.Format(_configuration["GeneralConfig:tablaAmortizacion:francesa"]);
                        break;
                }

            }
            else
                TipoTasaAux = request.idTipoTabla;
            ModificarSolicitudRequestMicroServ requestMicro = new ModificarSolicitudRequestMicroServ()
            {
                Opcion = "2",
                IdCanal = string.Format(_configuration["GeneralConfig:canal"]),
                IdProducto = producto.idProducto,
                IdTipoPeriodicidad = producto.tipoPeriodicidad,
                IdSolicitud = request.IdSolicitud,
                Usuario = string.Format(_configuration["GeneralConfig:usuario"]),
                MontoSolicitado = request.MontoSolicitado,
                PlazoSolicitado = request.PlazoSolicitado,
                IdTipoTabla = TipoTasaAux,
                TasaProducto = request.TasaProducto,
                ValorDividendo = request.ValorDividendo,
                ImagenReconocimientoBiometrico = request.ImagenReconocimientoBiometrico,
                PorcentajeReconocimientoBiometrico = request.PorcentajeReconocimientoBiometrico,
                RespuestaReconocimientoBiometrico = request.RespuestaReconocimientoBiometrico,
                FechaReconocimientoBiometrico = request.FechaReconocimientoBiometrico,
                NavegadorWeb = request.NavegadorWeb,
                DispositvoOrigen = request.DispositvoOrigen,
                VersionSoOrigen = request.VersionSoOrigen,
                TieneCamaraWeb = request.TieneCamaraWeb,
                DireccionIp = request.DireccionIp,
                DiaPago = request.DiaPago,
                CodigoCampania = request.CodigoCampania,
                IdTipoValidacionBiometrica = request.IdTipoValidacionBiometrica,
                CapacidadEndeudamientoMensual = request.CapacidadEndeudamientoMensual,
                MaximoDeudaConsumo = request.MaximoDeudaConsumo,
                MaximaDeudaTc = request.MaximaDeudaTc,
                IngresoFinal = request.IngresoFinal,
                SubProductoCore = request.SubProductoCore,
                IdSegmentoEstrategicoCore = request.IdSegmentoEstrategicoCore,
                PerfilCliente = request.PerfilCliente,
                SecuenciaRiesgo = request.SecuenciaRiesgo,
                NumIdentificacion = request.Identificacion,
                Nombre =  request.Nombre != null ? request.Nombre.Replace("'", "") : request.Nombre,
                CorreoElectronico = request.CorreoCore,
                IdExpediente = request.IdExpediente,
                AutorizaConsultaBuro = "",
                ValidaPreguntasSeguridad = "",
                RespuestaVelidaPreguntasSeguridad = "",
                FechaValidaPreguntasSeguridad = "",
                CupoSolicitado = "",
                DigitoInicialBinTarjeta = "",
                CorreoElectronicoCore = "",
                TieneRegistroFirma = "",
                NumeroSolicitudTarjetaCredito = "",
                PaisDireccionIp = "",
                AutenticadoBV = "",
                NuevoCliente = "",
                CelularCore = request.CelularCore,
                MontoTotalCredito = MoToCred,
                idEstadoActualSolicitud = request.idEstadoActualSolicitud,
                tieneActVenta = request.tieneActVenta,
                tieneActDirNegocio = request.tieneActDirNegocio,
                fuenteIngreso = request.fuenteIngreso
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/expedientes/solicitud/actualizacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<ModificarSolicitudResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ModificarSolicitudResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                modificarSolicitudResponse = _mapper.Map<ModificarSolicitudResponse>(responseJson.data);
                modificarSolicitudResponse.DataTransaccion.codigoResponse = 0;
                modificarSolicitudResponse.DataTransaccion.MessageResponse = "Modificación éxitosa";
            }
            else
            {
                if (request.GenerarException) { generarExceptionModificar = true; }
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    modificarSolicitudResponse.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                    modificarSolicitudResponse.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                }
                else
                {
                    modificarSolicitudResponse.DataTransaccion.MessageResponse = response.ReasonPhrase;
                    modificarSolicitudResponse.DataTransaccion.DescriptionResponse = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.Identificacion, request, modificarSolicitudResponse.DataTransaccion);

            if (generarExceptionModificar) { throw new GestionExpedienteException(modificarSolicitudResponse.DataTransaccion.MessageResponse, modificarSolicitudResponse.DataTransaccion.DescriptionResponse, modificarSolicitudResponse.DataTransaccion.codigoResponse); }


            return modificarSolicitudResponse;

        }

        public async Task<Producto> ObtenerProducto(string productoId)
        {
            Producto producto = PrimitiveDataUtils.obtenerProducto(productoId, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 3);
            }

            return producto;
        }

        public async Task<Producto> ObtenerIdProducto(string productoId)
        {
            Producto producto = PrimitiveDataUtils.obtenerIdProducto(productoId, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 3);
            }

            return producto;
        }
        public async Task<VerificaSolicitudesResponseMicro> RetomarSolicitud(RetomarSolicitudRequest request)
        {
            VerificaSolicitudesResponseMicro response = new VerificaSolicitudesResponseMicro();
    
            //if (String.IsNullOrEmpty(producto.idProducto))
            //{
            //    throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            //}
            request.idProducto = "0";//producto.idProducto;
            request.idCanal = string.Format(_configuration["GeneralConfig:canal"]);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());


            var responseHttp = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/solicitudes/solicitud/en-proceso";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            responseHttp = await client.PostAsync(uri, httpContent);

            string responseBody = await responseHttp.Content.ReadAsStringAsync();
            bool generarException = false;

            if (responseHttp.IsSuccessStatusCode)
            {
                MsResponse<VerificaSolicitudesResponseMicro> responseJson = JsonConvert.DeserializeObject<MsResponse<VerificaSolicitudesResponseMicro>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                response = _mapper.Map<VerificaSolicitudesResponseMicro>((VerificaSolicitudesResponseMicro)responseJson.data);
                response.DataTransaccion.codigoResponse = 0;
                response.DataTransaccion.MessageResponse = "Transacción exitosa";
                Producto producto = PrimitiveDataUtils.obtenerIdProducto(response.SolicitudesProcesoContratacion.Any()? response.SolicitudesProcesoContratacion[0].IdProducto: "" , _configuration);
                response.DtosProducto = producto;

            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    response.DataTransaccion.MessageResponse = responseJsonError.errors[0].message.ToString();
                    response.DataTransaccion.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";


                    if (responseJsonError.errors[0].code == 4)
                    {
                        response.DataTransaccion.MessageResponse = "Error en transaccion";
                        response.DataTransaccion.DescriptionResponse = "Error en transaccion";
                    }

                }
                else
                {
                    response.DataTransaccion.MessageResponse = responseHttp.ReasonPhrase;
                    response.DataTransaccion.DescriptionResponse = responseHttp.RequestMessage.ToString();
                }
            }

            #region LOGS
            string requestData = PrimitiveDataUtils.objectToString(request);
            string responseData = string.Empty;
            if (response.SolicitudesProcesoContratacion.Any())
            {
                responseData += PrimitiveDataUtils.objectToString(response.SolicitudesProcesoContratacion[0]);
            }
            else { response.DataTransaccion.DescriptionResponse = "No existen solicitudes en proceso de contratacion"; }

            responseData += PrimitiveDataUtils.objectToString(response.ContratarProducto);

            responseData += PrimitiveDataUtils.objectToString(response.DataTransaccion);

            PrimitiveDataUtils.saveLogsInformation(uri, request.numIdentificacion, request, response);
            #endregion

            if (generarException) { throw new GestionExpedienteException(response.DataTransaccion.MessageResponse, response.DataTransaccion.DescriptionResponse, response.DataTransaccion.codigoResponse); }

            return response;
        }

        public async Task<ConsultarSectorPymesResponse> SectoresVetados(string identificacion)
        {
            ConsultarSectorPymesResponse responseSector = new ConsultarSectorPymesResponse();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("identificacion", identificacion);
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();

            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/sector-pymes";
            response = await client.GetAsync(uri);

            string responseBody = await response.Content.ReadAsStringAsync();
            bool generarException = false;

            if (response.IsSuccessStatusCode)
            {

                MsResponse<ConsultarSectorPymesResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultarSectorPymesResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseSector = _mapper.Map<ConsultarSectorPymesResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, identificacion, responseSector);

            return responseSector;

        }

        public async Task<ConsultaGuidPersonaResponse> ConsultaGuidPersona(string identificacion)
        {            
            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = null;
            InformacionClienteDniResponse informacionClienteDniResponse = null;
            ConsultaGuidPersonaResponse consultaGUIDPersona = new ConsultaGuidPersonaResponse();

            var clientMicro = new HttpClient();
            clientMicro.DefaultRequestHeaders.Accept.Clear();
            clientMicro.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientMicro.DefaultRequestHeaders.Add("identificacion", identificacion);
            //clientMicro.DefaultRequestHeaders.Add("opcion", requestMicro.opcion.ToString());
            clientMicro.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroPersonas:url"]) + "v2/consolidada/identificacion";
            responseMicro = await clientMicro.GetAsync(uri);
            string responseBodyMicro = await responseMicro.Content.ReadAsStringAsync();
            MsResponse<ConsultaGuidPersonaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaGuidPersonaResponse>>(responseBodyMicro, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });


            if (responseMicro.IsSuccessStatusCode)
            {
                consultaGUIDPersona = _mapper.Map<ConsultaGuidPersonaResponse>(responseJson.data);

            }
            else
            {
                if ((int)responseMicro.StatusCode == 400)
                {
                    consultaGUIDPersona.guid_persona = "";
                }
                else
                {
                    throw new InformacionClienteExeption(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, identificacion, identificacion, consultaGUIDPersona);
            return consultaGUIDPersona;
        }

        public async Task<ValidaFirmaElectronicaResponse> ValidaCreaSolicitudesProducto(ValidaFirmaElectronicaRequest request)
        {
            ValidaFirmaElectronicaResponse validaFirmaElectronicaResponse = new ValidaFirmaElectronicaResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";
            logResp.codigoResponse = 7;
            #region valores de trama  a grabar
            List<string> atributosRequestIncluidos = new List<string>()
            {
                "identificacion"
            };
            #endregion
            uri = _configuration["InfraConfig:APIFirmaElectronica:urlFirmaElectronica"] + "FirmasElectronicas/ValidarCertificadosNoUtilizados";
            HttpResponseMessage response = await HTTPRequest.PostAsyncFirma(uri, request);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);
            if (response.IsSuccessStatusCode)
            {
                ValidaFirmaElectronicaResponse responseJson = JsonConvert.DeserializeObject<ValidaFirmaElectronicaResponse>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                validaFirmaElectronicaResponse = _mapper.Map<ValidaFirmaElectronicaResponse>((ValidaFirmaElectronicaResponse)responseJson);
                logResp.codigoResponse = 0;
                logResp.MessageResponse = "Consulta OK";
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, validaFirmaElectronicaResponse);
            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }
            return validaFirmaElectronicaResponse;

        }

        public async Task<ConsultarExpProductoActResponse> consultaExpedientesId(ConsultarExpProductoActRequest request)
        {
            Transaccion logResp = new Transaccion();
            ConsultarExpProductoActResponse responseConsulta = new ConsultarExpProductoActResponse();
            bool generarException = false;
            string uri = "";
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.descriptionProducto, _configuration);
             if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }


            request.descriptionProducto = string.Empty;
            request.idProductoPadre = producto.idProductoPadre;
            request.idProducto = producto.idProducto;
            responseConsulta.DataTransaccion = new Transaccion();
            responseConsulta.DataTransaccion.codigoResponse = 6; 
            #region Declaracion de Headers Request
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("idExpediente", request.idExpediente.ToString());
            headers.Add("idProductoPadre", producto.idProductoPadre);
            headers.Add("idProducto", request.idProducto);
            headers.Add("usuario", request.usuario);
            headers.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            #endregion
            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/expedientes/expediente/id";
            string auth = _configuration["AzureAd:tokenName"];
            HttpResponseMessage response = await HTTPRequest.GetAsync(uri, headers);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                MsResponse<ConsultarExpProductoActResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultarExpProductoActResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseConsulta = responseJson.data;
                responseJson.data.codigoRetorno = responseJson.data.infoProductoActivo.codigoRetorno;
                responseJson.data.mensaje = responseJson.data.infoProductoActivo.mensaje;
                logResp.codigoResponse = responseJson.data.codigoRetorno;
                logResp.MessageResponse = responseJson.data.mensaje;
                responseConsulta.DataTransaccion.codigoResponse = 0;
                responseConsulta.DataTransaccion.MessageResponse = "Consulta Expediente-Producto Activo exitosa";
                responseConsulta.DataTransaccion.DescriptionResponse = "Expediente N.- " + request.idExpediente;
            }
            else
            {
                throw new GeneralException(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.idExpediente.ToString(), request, responseConsulta);
            return responseConsulta;
        }

        public async Task<EncolarExpProcesoNeoBatchResponse> EncolarProcesoNeoBatch(EncolarExpProcesoNeoBatchRequest request)
        {
            string nombreMetodo = "EncolarProcesoNeoBatch";
            EncolarExpProcesoNeoBatchResponse encolarResponse = new EncolarExpProcesoNeoBatchResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";
            logResp.codigoResponse = 3;
            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/expedientes/procesoBatch";
            string auth = _configuration["AzureAd:tokenName"];
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);
            encolarResponse.CodigoRetorno = logResp.codigoResponse;
            encolarResponse.Mensaje = logResp.MessageResponse + ": " + logResp.DescriptionResponse;
            if (response.IsSuccessStatusCode)
            {
                MsResponse<EncolarExpProcesoNeoBatchResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<EncolarExpProcesoNeoBatchResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                encolarResponse = responseJson.data;
                logResp.codigoResponse = encolarResponse.CodigoRetorno;
                logResp.MessageResponse = encolarResponse.Mensaje;
                if (encolarResponse.CodigoRetorno != 0)
                {
                    generarException = true;
                    logResp.MessageResponse = $"Error Aplicativo en {nombreMetodo}";
                    logResp.DescriptionResponse = encolarResponse.Mensaje;
                    encolarResponse.Mensaje = logResp.MessageResponse + ": " + logResp.DescriptionResponse;
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, "", request,  encolarResponse);

            if (generarException && request.controlExcepcion) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }

            return encolarResponse;
        }

        public async Task<ActualizaExpedientesDetalleResponse> ActualizaExpedientesDetalles(ActualizaExpedientesDetalleRequest request)
        {
            ActualizaExpedientesDetalleResponse responseApi = new ActualizaExpedientesDetalleResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";
            logResp.codigoResponse = 3;
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.idProducto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }
            Transaccion responseExpediente = new Transaccion();

            request.idProducto = producto.idProducto;


            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/expedientes/expediente/detalles/actualizacion";
            string auth = _configuration["AzureAd:tokenName"];
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);
            if (response.IsSuccessStatusCode)
            {
                MsResponse<ActualizaExpedientesDetalleResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ActualizaExpedientesDetalleResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                responseApi = _mapper.Map<ActualizaExpedientesDetalleResponse>(responseJson.data);
                logResp.codigoResponse = 0;
                logResp.MessageResponse = "Consulta OK";
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "",  request,  response);

            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }
            return responseApi;
        }
    }
}