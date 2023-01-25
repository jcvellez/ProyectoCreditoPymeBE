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
    public class ArchivoIvaRestRepository: IArchivoIvaRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public ArchivoIvaRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<ArchivoImpuestoIvaResponse> ValidarArchivoIva(ArchivoImpuestoIvaRequest request)
        {
            ArchivoImpuestoIvaResponse archidoDataResponse = null;

            byte[] data = Convert.FromBase64String(request.file);
            MemoryStream memoryStream = new MemoryStream(data);

            PdfReader pdfReader = new PdfReader(memoryStream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            string textPdf = "";
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
            {
                textPdf += (string)PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
            }


            pdfDoc.Close();
            pdfReader.Close();
            var fecha = DateTime.UtcNow;//DateTime.Parse(request.fecha);
            var separarMes = request.mes.Split('-');
            string mes = separarMes[0];
            var anyo = Int32.Parse(separarMes[1]);
            textPdf = textPdf.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            ArchivoImpuestoIvaRequest requestArchivoIva = new ArchivoImpuestoIvaRequest()
            {
                identificacion = request.identificacion,
                fecha = fecha.ToString("yyyy-MM-dd"),
                file = textPdf,
                mes = mes.Trim(),
                anyo = anyo,
                idProceso = request.idProceso,
                ordenMes = request.ordenMes,
                tipo =request.tipo
            };

            var clientRating = new HttpClient();
            clientRating.DefaultRequestHeaders.Accept.Clear();
            clientRating.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/balance-iva";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestArchivoIva), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            clientRating.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await clientRating.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<ArchivoImpuestoIvaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ArchivoImpuestoIvaResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                archidoDataResponse = _mapper.Map<ArchivoImpuestoIvaResponse>(responseJson.data);
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

                        throw new ArchivoIvaExecption(mensajeError, mensajeError, code);

                    }
                }
                throw new ArchivoIvaExecption(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, archidoDataResponse);

            return archidoDataResponse;
        }

        public async Task<IngresoDeclaracionSemestralResponse> IngresoDeclaracionSemestral(IngresoDeclaracionSemestralRequest request)
        {
            IngresoDeclaracionSemestralResponse response = new();
            var clientRating = new HttpClient();
            clientRating.DefaultRequestHeaders.Accept.Clear();
            clientRating.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/gestion-manual-declaracioniva";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            clientRating.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await clientRating.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();


            if (responseMicro.IsSuccessStatusCode)
            {

                MsResponse<IngresoDeclaracionSemestralResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<IngresoDeclaracionSemestralResponse>>(responseRating, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                response = _mapper.Map<IngresoDeclaracionSemestralResponse>(responseJson.data);
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

                        throw new ArchivoIvaExecption(mensajeError, mensajeError, code);

                    }
                }
                throw new ArchivoIvaExecption(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, response);

            return response;
        }
    }
}
