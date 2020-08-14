using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Shared.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Restaurants.Shared
{
    public class JWTHandler
    {
        private const string USER_NAME = "UserName";
        private const string USER_ID = "UserId";

        private readonly IOptions<JWTSettings> JWTSettings;
        private readonly IConfiguration Configuration;

        public JWTHandler(IOptions<JWTSettings> jwtSettings, IConfiguration configuration)
        {
            JWTSettings = jwtSettings;
            Configuration = configuration;
        }
        public string GetJWT(User user)
        {
            try
            {
                var jwt = new JwtSecurityToken(
                    issuer: JWTSettings.Value.ValidIssuer,
                    audience: JWTSettings.Value.ValidAudience,
                    claims: new[]
                    {
                        new Claim(USER_NAME, user.Username),
                        new Claim(USER_ID, user.Id),
                    },
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddHours(GetExpiryHours(JWTSettings.Value.ExpirationInHours)),
                    signingCredentials: GetSigningCredentials());

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public (bool, string, string) CheckJWT(IHeaderDictionary headers)
        {
            if (headers.TryGetValue("JWTToken", out var tokenArr))
                return (false, null, null);

            if (tokenArr.Count > 0 && tokenArr[0] != null)
            {
                var token = tokenArr[0].Remove(0, 7);
                var tokenHandler = new JwtSecurityTokenHandler();
                if (tokenHandler.CanReadToken(token))
                {
                    return ValidateJWT(token, tokenHandler);
                }
            }
            return (false, null, null);
        }

        private (bool, string, string) ValidateJWT(string token, JwtSecurityTokenHandler tokenHandler)
        {
            try
            {
                var validationParameters = GetValidationParameters();
                _ = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var parsedToken = tokenHandler.ReadJwtToken(token);
                var username = parsedToken.Claims.First(claim => claim.Type == USER_NAME).Value;
                var userId = parsedToken.Claims.First(claim => claim.Type == USER_ID).Value;
                return (true, username, userId);
            }
            catch (Exception)
            {
                return (false, null, null);
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = JWTSettings.Value.ValidIssuer,
                ValidAudience = JWTSettings.Value.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["JWTSecurityKey"]))
            };
        }

        private static double GetExpiryHours(string value)
        {
            return double.TryParse(value, out double hours) ? hours : 24;
        }

        private SigningCredentials GetSigningCredentials()
        {
            //var SymmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["JWTSecurityKey"]));
            var SymmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String("Tc46Yj0bfhCPWYxpyD5Mfmqh0GaRIrZvwiO8AhdoG0R6UpVdNmpOX8j2XHi1vU9gAaMwjotZIdGy2nqb7PgxnRbe0ihktTYU5Mfb9tZZAoJHuS4fSUY2Jyqg4IK0tHXoFgNJXo5y9fAC8gVKCcF4GvGDmnEXVp5R4uhqKI7lEiqa3vt08MciuCfdqn0itQNemrp0iqGvSNdgKu7FDZbrzURU5LtU7zhSGzbwczFW4sdg0xyhXXgtpAYBeYgiRBE7"));
            return new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
