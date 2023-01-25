using bg.hd.banca.pyme.application.models.exeptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.infrastructure.seguridad
{
    public class TokenValidationHandler
    {
        public static void SetupJWTServices(WebApplicationBuilder builder)
        {
            string tokenName = builder.Configuration["Security:tokenName"];
            string key = builder.Configuration["Security:key"];
            var issuer = builder.Configuration["Security:issuer"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

                options.Events = new JwtBearerEvents
                {

                    OnMessageReceived = context =>
                    {

                        var accessToken = context.Request.Headers[tokenName].FirstOrDefault()?.Split(" ")?.Last() ?? string.Empty;
                        if (!string.IsNullOrEmpty(accessToken))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {

                        var accessToken = context.Request.Headers[tokenName].FirstOrDefault()?.Split(" ")?.Last() ?? string.Empty;
                        if (string.IsNullOrEmpty(accessToken))
                            throw new CustomUnauthorizedExeption("Unauthorized", "invalid_token");

                        if (context.Error.Equals("invalid_token"))
                            throw new CustomUnauthorizedExeption("Unauthorized", context.Error);

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType().Equals(typeof(CustomUnauthorizedExeption)))                           
                        return Task.CompletedTask;              
                        
                        throw new CustomUnauthorizedExeption("Unauthorized", context.Exception.GetType().Name);                                              
                    }
                };
            });
        }
    }
}
