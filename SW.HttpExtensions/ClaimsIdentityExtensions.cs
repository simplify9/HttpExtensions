//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace SW.HttpExtensions
//{
//    public static class ClaimsIdentityExtensions
//    {

//        public static string GenerateJwt(this ClaimsIdentity claimsIdentity, string key, string issuer, string audience)
//        {

//            if (key == null) return null;
//            if (issuer == null) return null;
//            if (audience == null) return null;
//            if (!claimsIdentity.IsAuthenticated) return null;

//            var td = new SecurityTokenDescriptor
//            {
//                Subject = claimsIdentity,
//                Expires = DateTime.UtcNow.AddDays(7),
//                Issuer = issuer,
//                Audience = audience,
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
//            };

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var token = tokenHandler.CreateToken(td);
//            return tokenHandler.WriteToken(token);
//        }
//    }
//}
