using AutoMapper;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.application.models.ms;
using bg.hd.banca.pyme.domain.entities.email;
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
using bg.hd.banca.pyme.infrastructure.utils;
using bg.hd.banca.pyme.application.interfaces.services;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    public class EmailValidacionRestRepository: IEmailValidacionRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public EmailValidacionRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }
        public async Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request)
        {
            EmailValidacionResponse emailValidacionResponse = null;

            if (!PrimitiveDataUtils.ValidarEstructuraEmail(request.correo))
            {
                throw new EmailValidacionExecption("Correo no valido", "Correo no valido", 1);
            }

            EmailValidacionRequest requestMicroEmail = new EmailValidacionRequest()
            {
                codigoAppBg = _configuration["InfraConfig:MicroClientes:codigoAppBg"],
                identificacion = request.identificacion,
                correo = request.correo,
                
            };


            var clientMicroMail = new HttpClient();
            clientMicroMail.DefaultRequestHeaders.Accept.Clear();
            clientMicroMail.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroClientes:urlAutorizaciones"]) + "v1/email/validacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicroEmail), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            clientMicroMail.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await clientMicroMail.PostAsync(uri, httpContent);
            string response = await responseMicro.Content.ReadAsStringAsync();

            MsResponse<EmailValidacionResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<EmailValidacionResponse>>(response, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            if (responseMicro.IsSuccessStatusCode)
            {
                emailValidacionResponse = _mapper.Map<EmailValidacionResponse>(responseJson.data);
                emailValidacionResponse.CodigoRetorno = 0;
                emailValidacionResponse.Mensaje = emailValidacionResponse.descripcionValidacion;

                if (!emailValidacionResponse.estadoValidacion.Equals("V"))
                {
                    throw new EmailValidacionExecption(emailValidacionResponse.descripcionValidacion, emailValidacionResponse.descripcionValidacion.ToString(), 1);
                }

            }
            else
            {
                throw new EmailValidacionExecption(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, emailValidacionResponse);

            return emailValidacionResponse;

        }

    }
}
