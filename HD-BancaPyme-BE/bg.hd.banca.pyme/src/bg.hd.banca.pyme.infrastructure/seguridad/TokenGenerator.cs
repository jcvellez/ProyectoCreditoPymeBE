using bg.hd.banca.pyme.application.interfaces.services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bg.hd.banca.pyme.infrastructure.seguridad
{
    public class TokenGenerator : ITokenGenerator
    {

        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public string GenerateTokenJwt(string username, out DateTime ExpireTime, out int ExpireIn)
        {

            string key = _configuration["Security:key"];
            var issuer = _configuration["Security:issuer"];
            var expireTime = _configuration["Security:expiresin"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("clientid", username)
            };

            ExpireTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime));

            var token = new JwtSecurityToken(issuer,
                            issuer,
                            permClaims,
                            expires: ExpireTime,
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);


            ExpireIn = 60 * int.Parse(expireTime);

            return jwt_token;
        }

    }
}
