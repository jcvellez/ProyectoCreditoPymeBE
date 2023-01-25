using AutoMapper;
using bg.hd.banca.pyme.infrastructure.utils;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.documento;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class DocumentoRestRepository : IDocumentoRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public DocumentoRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<DigitalizarDocumentosResponse> DigitalizarDocumentos(DigitalizarDocumentosRequest request)
        {
            //Log.Information("{Proceso} {idexpediente}", "DigitalizarDocumentos IN", "");
            Documento documento = PrimitiveDataUtils.obtenerDocumento(request.Id, _configuration);
            if (string.IsNullOrEmpty(documento.idDocumento))
            {
                throw new GeneralException("No se ha encontrado el documento", "El ID no esta permitido", 2);
            }

            // request.Usuario = _configuration["InfraConfig:MicroCompositeNeo:ServiceDocumento:usuario"];
            request.IdDocumento = Convert.ToInt32(documento.idDocumento);
            request.NombreDocumento = documento.nombreDocumento;
            request.ProcesaImagen = bool.Parse(documento.procesaImagen);
            request.TamanioMaximoDocumento = Convert.ToInt32(documento.tamanioMaximoDocumento);
            request.Usuario = string.Format(_configuration["GeneralConfig:usuarioWeb"]);
            DigitalizarDocumentosResponse generarResponse = new DigitalizarDocumentosResponse();

            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/documentos/documento/digitalizacion";
            string auth = string.Format(_configuration["AzureAd:tokenName"]);
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();
            Transaccion logResp = new Transaccion();
            bool generarException = HTTPRequest.ObtenerErrores(uri, response, responseBody, logResp);

            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<DigitalizarDocumentosResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<DigitalizarDocumentosResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                generarResponse = responseJson.data;
                logResp.codigoResponse = generarResponse.CodigoRetorno;
                logResp.MessageResponse = generarResponse.Mensaje;
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, generarResponse);

            if (generarException)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);

            }

            return generarResponse;
        }


        public async Task<GenerarDocumentosCreditoResponse> GenerarDocumentosContratoscredito(GenerarDocumentosCreditoRequest request)
        {
            GenerarDocumentosCreditoResponse generarResponse = new GenerarDocumentosCreditoResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
               throw new GeneralException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            } 

            request.producto = producto.idProducto;
            uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/documentos/documento/contratos-credito";
            string auth = _configuration["AzureAd:tokenName"];
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<GenerarDocumentosCreditoResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<GenerarDocumentosCreditoResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                generarResponse = responseJson.data;
                logResp.codigoResponse = generarResponse.CodigoRetorno;
                logResp.MessageResponse = generarResponse.Mensaje;
            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    logResp.MessageResponse = responseJsonError.errors[0].message.ToString();
                    logResp.DescriptionResponse = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";



                }
                else
                {
                    logResp.MessageResponse = response.ReasonPhrase;
                    logResp.DescriptionResponse = response.RequestMessage.ToString();

                }
            }
            if (generarException)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);
            }

            return generarResponse;
        }

    }
}
