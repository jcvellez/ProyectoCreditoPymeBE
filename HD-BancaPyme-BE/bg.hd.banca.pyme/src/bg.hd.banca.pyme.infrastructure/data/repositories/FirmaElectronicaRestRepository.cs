using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities.firmaElectronica;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class FirmaElectronicaRestRepository : IFirmaElectronicaRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;
        public FirmaElectronicaRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<GenerarCertificadoFirmaElectronicaResponse> GenerarCertificadoFirmaElectronica(GenerarCertificadoFirmaElectronicaRequest request)
        {
            GenerarCertificadoFirmaElectronicaResponse solicitarCertFirmaElectronicaResponse = new GenerarCertificadoFirmaElectronicaResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";
            string auth = _configuration["AzureAd:tokenName"];
            uri = _configuration["InfraConfig:APIFirmaElectronica:urlFirmaElectronica"] + "FirmasElectronicas/SolicitarCertificado";
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                GenerarCertificadoFirmaElectronicaResponse responseJson = JsonConvert.DeserializeObject<GenerarCertificadoFirmaElectronicaResponse>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                solicitarCertFirmaElectronicaResponse = _mapper.Map<GenerarCertificadoFirmaElectronicaResponse>((GenerarCertificadoFirmaElectronicaResponse)responseJson);
                logResp.codigoResponse = 0;
                logResp.MessageResponse = "Consulta OK";
            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {
                    MsDtoResponseError responseErrrorJson = new MsDtoResponseError();
                    responseErrrorJson = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    if (responseErrrorJson.errors.Count == 1)
                    {
                        logResp.codigoResponse = responseErrrorJson.errors[0].code;
                        logResp.DescriptionResponse = responseErrrorJson.errors[0].message;
                        logResp.MessageResponse = responseErrrorJson.message;
                    }
                    else
                    {
                        logResp.codigoResponse = 1;
                        logResp.DescriptionResponse = "Error Aplicativo";
                        logResp.MessageResponse = "Error Aplicativo";
                    }
                }
                else
                {
                    logResp.codigoResponse = ((int)response.StatusCode);
                    logResp.DescriptionResponse = response.RequestMessage == null ? "Error Aplicativo" : response.RequestMessage.ToString();
                    logResp.MessageResponse = "Error Aplicativo";
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion,  request, solicitarCertFirmaElectronicaResponse);

            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }
            return solicitarCertFirmaElectronicaResponse;
        }

        public async Task<RegistrarAuditoriaResponse> RegistrarAuditoriaFirmaElectronica(RegistrarAuditoriaRequest request)
        {
            RegistrarAuditoriaResponse regauditoriaResponse = new RegistrarAuditoriaResponse();

            RegistrarAuditoriaMicroServRequest requestMicroServ = new RegistrarAuditoriaMicroServRequest()
            {
                opcion = request.opcion,
                idExpediente = request.idExpediente,
                idAccion = request.idAccion,
                idDocumento  = request.idDocumento,
                usuario = request.usuario,
                ip = request.ip,
                comentario = request.comentario
            };

            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";
            logResp.codigoResponse = 2;
            
            string nombreAccion = request.accion;
            DocumentoFirma[] lista_apsett = _configuration.GetSection("FirmaElectronica:listaAcciones").Get<DocumentoFirma[]>();            
            foreach (DocumentoFirma accionAUX in lista_apsett)
            {
                if (accionAUX.accion.Equals(nombreAccion)) { 
              
                    if (accionAUX.accion.Equals("ver-documento")) {
                        requestMicroServ.idAccion = accionAUX.valor;
                        requestMicroServ.comentario = accionAUX.mensaje;
                        break; }
                    if (accionAUX.accion.Equals("descargar-documento")) {
                        requestMicroServ.idAccion = accionAUX.valor;
                        requestMicroServ.comentario = accionAUX.mensaje;
                        break; }
                    if (accionAUX.accion.Equals("crea-firmaelectronica")) { 
                        requestMicroServ.idAccion = accionAUX.valor;
                        requestMicroServ.comentario = accionAUX.mensaje;
                        break; }
                    if (accionAUX.accion.Equals("firma-documento")) { 
                        requestMicroServ.idAccion = accionAUX.valor;
                        requestMicroServ.comentario = accionAUX.mensaje;
                        break; }
                    if (accionAUX.accion.Equals("aceptar-firmaelectronica")) {
                        requestMicroServ.idAccion = accionAUX.valor;
                        requestMicroServ.comentario = accionAUX.mensaje;
                        break; }
                }
            }            
            
            string auth = _configuration["AzureAd:tokenName"];
            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/firma-electronica/auditoria";
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), requestMicroServ);
            string responseBody = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode.Equals(false)) { generarException = true; }
            
            if (response.IsSuccessStatusCode)
            {
                RegistrarAuditoriaResponse responseJson = JsonConvert.DeserializeObject<RegistrarAuditoriaResponse>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                regauditoriaResponse = _mapper.Map<RegistrarAuditoriaResponse>((RegistrarAuditoriaResponse)responseJson);
                regauditoriaResponse.CodigoRetorno = 0;
                regauditoriaResponse.Mensaje = "Registro de auditoría realizado con éxito.";
                logResp.codigoResponse = 0;
                logResp.MessageResponse = "Consulta OK";
            }else
            if ((int)response.StatusCode == 400)
            {
                logResp.codigoResponse = logResp.codigoResponse;
                logResp.DescriptionResponse = "Error en registro de audiroria";
            }

            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }
            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, regauditoriaResponse);
            return regauditoriaResponse;
        }

        public async Task<ConfirmacionCertificadoResponse> ConfirmarCertificadoFirmaElectronica(ConfirmacionCertificadoRequest request)
        {
            ConfirmacionCertificadoResponse confirmacionCertificadoResponse = new();
            string uri = "";
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 3);
            }

            uri = _configuration["InfraConfig:APIFirmaElectronica:urlFirmaElectronica"] + "FirmasElectronicas/IniciarProcesoFirmaPersonaNatural";
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, request);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                ConfirmacionCertificadoResponse responseJson = JsonConvert.DeserializeObject<ConfirmacionCertificadoResponse>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                confirmacionCertificadoResponse = _mapper.Map<ConfirmacionCertificadoResponse>((ConfirmacionCertificadoResponse)responseJson);
                if (confirmacionCertificadoResponse.CodigoRetorno != 0)
                {
                    generarException = true;
                    logResp.codigoResponse = confirmacionCertificadoResponse.CodigoRetorno;
                    logResp.MessageResponse = confirmacionCertificadoResponse.Mensaje; 
                    logResp.DescriptionResponse = confirmacionCertificadoResponse.Mensaje;
                    
                }
            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {
                    MsDtoResponseError responseErrrorJson = new MsDtoResponseError();
                    responseErrrorJson = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    if (responseErrrorJson.errors.Count == 1)
                    {
                        logResp.codigoResponse = responseErrrorJson.errors[0].code;
                        logResp.DescriptionResponse = responseErrrorJson.errors[0].message;
                        logResp.MessageResponse = responseErrrorJson.message;
                    }
                    else
                    {
                        logResp.codigoResponse = 1;
                        logResp.DescriptionResponse = "Error Aplicativo";
                        logResp.MessageResponse = "Error Aplicativo";
                    }
                }
                else
                {
                    logResp.codigoResponse = ((int)response.StatusCode);
                    logResp.DescriptionResponse = response.RequestMessage == null ? "Error Aplicativo" : response.RequestMessage.ToString();
                    logResp.MessageResponse = "Error Aplicativo";
                }
            }

            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }

            return confirmacionCertificadoResponse;
        }
    }
}