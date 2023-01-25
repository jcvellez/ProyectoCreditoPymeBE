using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.ArchivosImpuesto;
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
    public class ArchivosImpuestoRestRepository : IArchivosImpuestoRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public ArchivosImpuestoRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<ArchivosImpuestoResponse> ValidarArchivoImpuesto(ArchivosImpuestoRequest request)
        {
            ArchivosImpuestoResponse archidoData = null;

            byte[] data = Convert.FromBase64String(request.file);
            string decodedString = Encoding.UTF8.GetString(data);

            MemoryStream memoryStream = new MemoryStream(data);
            string textPdf = "";
            PdfReader pdfReader = new PdfReader(memoryStream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
            {
                textPdf += (string)PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
            }


            pdfDoc.Close();
            pdfReader.Close();
            textPdf = textPdf.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            ArchivosImpuestoRequest requestArchivoRenta = new ArchivosImpuestoRequest()
            {
                identificacion = request.identificacion,
                opcion = "PYM",
                usuario = _configuration["GeneralConfig:usuarioWeb"],
                idProceso = request.idProceso,
                fechaRevision = request.fechaRevision,
                file = textPdf,
                anyo = request.anyo
            };

            var clientRating = new HttpClient();
            clientRating.DefaultRequestHeaders.Accept.Clear();
            clientRating.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroRating:url"]) + "v2/analisis/balance";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestArchivoRenta), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            clientRating.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await clientRating.PostAsync(uri, httpContent);
            string responseRating = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<ArchivosImpuestoResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ArchivosImpuestoResponse>>(responseRating, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            

            if (responseMicro.IsSuccessStatusCode)
            {
                archidoData = _mapper.Map<ArchivosImpuestoResponse>(responseJson.data);
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

                        throw new ArchivosImpuestoExeption(mensajeError, mensajeError, code);

                    }
                }
                throw new ArchivosImpuestoExeption(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, archidoData);

            return archidoData;
        }
    }
}
