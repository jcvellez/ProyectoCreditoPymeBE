using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.token.acceso;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class TokenAccesoRestRepository : ITokenAccesoRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public TokenAccesoRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<ValidaTokenAccesoCanalApliResponse> ValidarTokenAccesoCanalAplicacion(ValidaTokenAccesoCanalApliRequest request)
        {
            ValidaTokenAccesoCanalApliResponse generarResponse = new ValidaTokenAccesoCanalApliResponse();
            Transaccion logResp = new Transaccion();
            bool generarException = false;
            string uri = "";

            uri = _configuration["InfraConfig:MicroClientes:urlAutorizaciones"] + "v1/token";
            #region Declaracion de Headers Request
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("CanalId", request.CanalId);
            headers.Add("AplicacionId", request.AplicacionId);
            headers.Add("Token", request.TkValue);
            headers.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            #endregion            
            HttpResponseMessage response = await HTTPRequest.GetAsync(uri, headers);
            string responseBody = await response.Content.ReadAsStringAsync();
            generarException = HTTPRequest.ObtenerErrores(response, responseBody, logResp);

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ValidaTokenAccesoCanalApliMICROResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ValidaTokenAccesoCanalApliMICROResponse>>(responseBody);
                generarResponse.CodigoRetorno = responseJson.data.CodigoRetorno;
                generarResponse.Mensaje = responseJson.data.mensajeRetorno;
                generarResponse.metaJson = JsonConvert.DeserializeObject<MetaJson>(responseJson.data.metaJson);
                string productoId = "";
                if (!string.IsNullOrEmpty(generarResponse.metaJson.producto))
                {
                    productoId = generarResponse.metaJson.producto;
                }
                Producto producto = PrimitiveDataUtils.obtenerIdProducto(productoId, _configuration);
                if (producto != null)
                {
                    generarResponse.metaJson.producto = producto.nombreProducto;
                }

                logResp.codigoResponse = generarResponse.CodigoRetorno;
                logResp.MessageResponse = generarResponse.Mensaje;
                logResp.DescriptionResponse = "Metodo ValidarTokenAccesoCanalAplicacion - Validacion Correcta";

            }
            PrimitiveDataUtils.saveLogsInformation(uri, request.TkValue, request, generarResponse);
            if (generarException)
            {
                throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse);
            }
            return generarResponse;
        }
    }
}
