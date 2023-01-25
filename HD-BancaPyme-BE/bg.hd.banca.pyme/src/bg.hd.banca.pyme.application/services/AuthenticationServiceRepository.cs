using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.security;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.services
{
    public class AuthenticationServiceRepository : IAuthenticationServiceRepository
    {
        private readonly IConfiguration _configuration;
        private static AuthenticationResponse? _authenticationMemory;
        public AuthenticationServiceRepository(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }
        public async Task<string> GetAccessToken()
        {
            string responseToken = string.Empty;
            if (_authenticationMemory == null || (_authenticationMemory != null && _authenticationMemory.expired_token != null && _authenticationMemory.expired_token <= DateTime.Now))
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = new HttpResponseMessage();

                string uri = string.Format("{0}{1}/oauth2/token", _configuration["AzureAd:Instance"], _configuration["AzureAd:TenantId"]);

                var data = new[]
                {
                new KeyValuePair<string, string>("grant_type", _configuration["AzureAd:GrantType"]),
                new KeyValuePair<string, string>("client_id", _configuration["AzureAd:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _configuration["AzureAdClientSecret"]), 
                new KeyValuePair<string, string>("resource", "api://"+_configuration["AzureAd:Audience"])
                };
                DateTime dateTimeLogin = DateTime.Now;
                response = await client.PostAsync(uri, new FormUrlEncodedContent(data));
                string? responseBody = await response.Content.ReadAsStringAsync();
                AuthenticationResponse? authResult = null;
                if (!string.IsNullOrEmpty(responseBody))
                {
                    authResult = JsonConvert.DeserializeObject<AuthenticationResponse?>(responseBody);

                }
                if (response.IsSuccessStatusCode)
                {
                    responseToken = authResult != null && authResult.access_token != null ? authResult.access_token : string.Empty;
                    if (authResult != null && authResult.expires_in != null)
                    {
                        authResult.expired_token = dateTimeLogin.AddSeconds(double.Parse(authResult.expires_in));
                        _authenticationMemory = authResult;
                    }
                }
                else
                {
                    throw new AuthenticationServiceException(authResult?.error, authResult?.error_description, ((int)response.StatusCode));
                }
            }
            else
                responseToken = _authenticationMemory != null && _authenticationMemory.access_token != null ? _authenticationMemory.access_token : string.Empty;

            if (!responseToken.StartsWith("Bearer "))
            {
                responseToken = "Bearer " + responseToken;
            }
            return responseToken;
        }
    }
}
