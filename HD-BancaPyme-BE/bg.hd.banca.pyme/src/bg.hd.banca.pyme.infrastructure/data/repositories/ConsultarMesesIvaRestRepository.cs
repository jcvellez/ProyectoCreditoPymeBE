using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;
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
using bg.hd.banca.pyme.infrastructure.utils;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using bg.hd.banca.pyme.application.interfaces.services;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    public class ConsultarMesesIvaRestRepository : IConsultaMesesIvaRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public ConsultarMesesIvaRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<ConsultarMesesIvaResponse> ConsultarMesesIva(int tipoConsulta, int idProceso)
        {
            ConsultarMesesIvaResponse dataResponse = null;

            ConsultarMesesIvaResquest requestMeses = new ConsultarMesesIvaResquest()
            {
                consultaAnyos = tipoConsulta == null ? 1 : tipoConsulta,
                idProceso = idProceso == null ? 0 : idProceso
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/balance-meses";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMeses), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await client.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<ConsultarMesesIvaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultarMesesIvaResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                dataResponse = _mapper.Map<ConsultarMesesIvaResponse>(responseJson.data);

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

            PrimitiveDataUtils.saveLogsInformation(uri, "", requestMeses, dataResponse);

            return dataResponse;
        }
    }
}
