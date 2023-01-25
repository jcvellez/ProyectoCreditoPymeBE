using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Linq;
using static bg.hd.banca.pyme.infrastructure.utils.PrimitiveDataUtils;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class IdentificaUsuarioRestRepository : IIdentificaUsuarioRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public IdentificaUsuarioRestRepository(IConfiguration Configuration, IMapper Mapper)
        {
            _configuration = Configuration;
            _mapper = Mapper;
        }

        public async Task<IdentificaUsuarioResponse> IdentificarUsuario(string identificacion, string codDactilar, string tipoId, bool obfuscate)
        {
            

            IdentificaUsuarioRequest request = new IdentificaUsuarioRequest()
            {
                ClienteId = identificacion,
                TipoId = tipoId,
            };
           
            IdentificaUsuarioResponse identificaUsuarioResponse = null;
            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = null;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("clienteid", identificacion);
            client.DefaultRequestHeaders.Add("tipoid", tipoId);


            var response = new HttpResponseMessage();            
            string uri = string.Format(_configuration["InfraConfig:MicroClientes:urlAutorizaciones"]) + "v2/contacto";
            response = await client.GetAsync(string.Format(_configuration["InfraConfig:MicroClientes:urlAutorizaciones"] + "v2/contacto"));
            string responseBody = await response.Content.ReadAsStringAsync();
            MsResponse<MsClienteInformacion> responseJson = JsonConvert.DeserializeObject<MsResponse<MsClienteInformacion>>(responseBody, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (response.IsSuccessStatusCode)
            {
                identificaUsuarioResponse = _mapper.Map<IdentificaUsuarioResponse>((MsClienteInformacion)responseJson.data);
                identificaUsuarioResponse.CelularOfuscado = PrimitiveDataUtils.ObfuscateType(identificaUsuarioResponse.CelularOfuscado, typeTag.Telefono);
                var query = new Dictionary<string, string>()
                {
                    ["identificacion"] = identificacion,
                };
                var clientSegmento = new HttpClient();
                clientSegmento.DefaultRequestHeaders.Accept.Clear();
                clientSegmento.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseSegmento = new HttpResponseMessage();
                responseSegmento = await clientSegmento.GetAsync(QueryHelpers.AddQueryString(string.Format(_configuration["InfraConfig:MicroClientes:urlServiceIdentificacionSegmento"] + "v1/informacion"), query));
                string responseBodySeg = await responseSegmento.Content.ReadAsStringAsync();

                MsResponse<IdentificaUsuarioDataResponse> responseJsonCliente = JsonConvert.DeserializeObject<MsResponse<IdentificaUsuarioDataResponse>>(responseBodySeg, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                identificaUsuarioDataResponse = _mapper.Map<IdentificaUsuarioDataResponse>(responseJsonCliente.data);


                var TwoWords = identificaUsuarioDataResponse.telefonoCelular.Substring(0,2);
                
                if (identificaUsuarioDataResponse.telefonoCelular == "0" || identificaUsuarioDataResponse.telefonoCelular == "0000000000" 
                    || !identificaUsuarioDataResponse.telefonoCelular.Any(c => char.IsDigit(c)) || identificaUsuarioDataResponse.telefonoCelular.Length < 9
                    || TwoWords == "00")
                {
                     throw new IdentificarUsuarioException("Número de celular invalido(vacio) ", "Número de celular invalido(vacio)", 1);
                }

            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    //throw new IdentificarUsuarioException("Cliente no existe", "Cliente no existe", 3);
                    IdentificaUsuarioResponse IdentificaUsuarioResponse400 = new IdentificaUsuarioResponse();
                    return IdentificaUsuarioResponse400;
                }
                else
                {

                    throw new IdentificarUsuarioException(responseJson.error.userMessage, responseJson.error.userMessage, ((int)response.StatusCode));
                }
            }

            if (obfuscate)
            {
                string textoAux = string.Format(_configuration["GeneralConfig:PalabrasReservadas"]);
                identificaUsuarioResponse.PrimerNombre = PrimitiveDataUtils.ObfuscateTags(identificaUsuarioResponse.PrimerNombre, PrimitiveDataUtils.typeTag.Nombre, textoAux);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, identificaUsuarioResponse);

            return identificaUsuarioResponse;
        }

    }
}
