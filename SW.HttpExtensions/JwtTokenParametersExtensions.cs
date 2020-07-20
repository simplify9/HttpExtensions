using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SW.HttpExtensions
{
    public static class JwtTokenParametersExtensions
    {
        public static string WriteJwt(this JwtTokenParameters jwtTokenParameters, ClaimsIdentity claimsIdentity)
        {
            if (!jwtTokenParameters.IsValid()) return null;

            var td = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtTokenParameters.Issuer,
                Audience = jwtTokenParameters.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenParameters.Key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(td);
            return tokenHandler.WriteToken(token);
        }

        public static ClaimsPrincipal ReadJwt(this JwtTokenParameters jwtTokenParameters, string jwt)
        {
            if (!jwtTokenParameters.IsValid()) return null;

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtTokenParameters.Issuer,
                ValidAudience = jwtTokenParameters.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenParameters.Key))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(jwt, tokenValidationParameters, out _);
        }
    }
}
