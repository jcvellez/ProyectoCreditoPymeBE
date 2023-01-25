using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.domain.entities.config;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace bg.hd.banca.pyme.infrastructure.utils
{
    public class HTTPRequest
    {
        private static readonly HttpClient _httpClient;

        static HTTPRequest()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string uri, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            var content = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response;
        }


        public static async Task<HttpResponseMessage> PostAsyncFirma<T>(string uri, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            var content = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, string auth, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            request.Headers.Add(auth, token);
            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, string auth, string token, string headerName, string headerValue)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            request.Headers.Add(auth, token);
            request.Headers.Add(headerName, headerValue);
            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }


        public static async Task<string> PostAsync<T>(string uri, string token, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string uri, string auth, string token, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            //_httpClient.DefaultRequestHeaders.Add(auth, token);
            request.Headers.Add(auth, token);
            var content = JsonConvert.SerializeObject(data);
            //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8);
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.SendAsync(request);
            //var response = await _httpClient.PostAsync(uri, httpContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(uri,response, responseBody);
            //response.EnsureSuccessStatusCode();
            return response;
        }
        public static bool ObtenerErrores(string uri, HttpResponseMessage response, string responseBody, Transaccion logResp)
        {
            bool generarException = false;
            MsDtoResponseError responseErrrorJson = new MsDtoResponseError();

            if (!response.IsSuccessStatusCode)
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {
                    responseErrrorJson = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    if (responseErrrorJson.errors.Count == 1)
                    {
                        logResp.codigoResponse = responseErrrorJson.errors[0].code;
                        logResp.DescriptionResponse = "Error Aplicativo";
                        logResp.MessageResponse = responseErrrorJson.errors[0].message;
                    }
                    //throw new GeneralException(mensajeError, mensajeError, code);
                }
                else
                {
                    logResp.codigoResponse = ((int)response.StatusCode);
                    logResp.DescriptionResponse = "Error Aplicativo";
                    logResp.MessageResponse = response.RequestMessage.ToString();

                }
            }
            //if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }
            //PrimitiveDataUtils.saveLogsInformation(uri, data.identificacion, data, responseJson.data);

            return generarException;

        }

        public static bool ObtenerErrores(HttpResponseMessage response, string responseBody, Transaccion logResp)
        {
            bool generarException = false;
            MsDtoResponseError responseErrrorJson = new MsDtoResponseError();

            if (!response.IsSuccessStatusCode)
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {
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
            return generarException;

        }

        public static void manejarErrores(string uri, HttpResponseMessage response, string responseBody, Transaccion logResp)
        {
            bool generarException = false;
            MsDtoResponseError responseErrrorJson = new MsDtoResponseError();

            if (!response.IsSuccessStatusCode)
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {
                    responseErrrorJson = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    if (responseErrrorJson.errors.Count == 1)
                    {
                        logResp.codigoResponse = responseErrrorJson.errors[0].code;
                        logResp.DescriptionResponse = "Error Aplicativo";
                        logResp.MessageResponse = responseErrrorJson.errors[0].message;
                    }
                    //throw new GeneralException(mensajeError, mensajeError, code);
                }
                else
                {
                    logResp.codigoResponse = ((int)response.StatusCode);
                    logResp.DescriptionResponse = "Error Aplicativo";
                    logResp.MessageResponse = response.RequestMessage.ToString();

                }
            }
            if (generarException) { throw new GeneralException(logResp.MessageResponse, logResp.DescriptionResponse, logResp.codigoResponse); }
            //PrimitiveDataUtils.saveLogsInformation(uri, data.identificacion, data, responseJson.data);



        }

        public static async Task<T> GetAsyncPersonalizado<T>(string uri, T objData, rsConsultaRUCPersona objError)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            return manejarRespuesta(response, responseBody, objData, objError);
        }

        static T manejarRespuesta<T>(HttpResponseMessage response, string responseBody, T objData, rsConsultaRUCPersona objError)
        {
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });

            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    var errorJson = JsonConvert.DeserializeObject<rsConsultaRUCPersona>(responseBody, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None
                    });
                    if ((int)errorJson.error.status == 400)
                    {
                        throw new GeneralException("Error Aplicativo", errorJson.error.userMessage.ToString(), Convert.ToInt32(errorJson.error.errorCode));
                    }
                    throw new GeneralException(response.ReasonPhrase, response.RequestMessage.ToString(), ((int)response.StatusCode));
                }
                else
                {
                    throw new GeneralException(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }

        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, IDictionary<string, string> headers)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            foreach (var kvp in headers)
            {
                request.Headers.Add(kvp.Key, kvp.Value);

            }
            var response = await _httpClient.SendAsync(request);
            return response;
        }
    }
}
