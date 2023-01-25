using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.domain.entities.recaptcha;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.VisualBasic;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class RecaptchaRestRepository : IRecaptchaRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public RecaptchaRestRepository(IConfiguration Configuration, IMapper Mapper)
        {
            _configuration = Configuration;
            _mapper = Mapper;
        }
        public async Task<RecaptchaResponse> validarRecaptcha(RecaptchaRequest request)
        {

            Log.Information("{Proceso} {Canal}", "Recaptcha IN", request.token);

            //DataSet _ds = new();
            RecaptchaResponse recaptchaResponse = new RecaptchaResponse();

            RecaptchaRequestMicroServ requestMicro = new RecaptchaRequestMicroServ()
            {
                secretKey = _configuration["GeneralConfig:secretKeyRecaptcha"],
                token = request.token,
                remoteIp = request.remoteIp
            };
            #region validaCampoSecretKey
            if (requestMicro.secretKey.Trim() == "" || requestMicro.secretKey is null)
            {
                throw new RecaptchaExeption("secretKey es requerida", "secretKey es requerida - Revisar el appsettings", 2);
            }
            #endregion

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroClientes:urlAutorizaciones"]) + "v2/recaptcha/validacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicro), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<RecaptchaResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<RecaptchaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                recaptchaResponse.CodigoRetorno = 2;
                if (responseJson.data.success)
                {
                    recaptchaResponse.CodigoRetorno = 0;
                    recaptchaResponse.Mensaje = "Validacion Recaptcha Exitosa";
                }
            }
            else
            {
                MsDtoResponseError responseJson = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                string mensajeError = ""; int cont = 0; int code = 2;
                if (responseJson.errors.Count == 1) mensajeError = responseJson.errors[0].message;
                else
                {
                    foreach (MsDtoError error in responseJson.errors)
                    {
                        cont += 1;
                        mensajeError += $"Error {cont}: {error.message} \n";
                    }
                }
                throw new RecaptchaExeption("Error Aplicativo", mensajeError, code);
            }

            return recaptchaResponse;
        }
    }
}
