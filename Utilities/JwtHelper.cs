using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Security.Cryptography;

namespace CarRentalSystem.Utilities
{
    public class JwtHelper
    {

        //public static string GenerateSecureKey(int keySize = 256)
        //{
        //    using (var random = new RNGCryptoServiceProvider())
        //    {
        //        byte[] keyBytes = new byte[keySize / 8];  // Convert the key size
        //        random.GetBytes(keyBytes);  // Get random bytes
        //        return Convert.ToBase64String(keyBytes);  // Convert to Base64 string
        //    }
        //}
        public static string GenerateToken(string email, string role, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var signingKey = new SymmetricSecurityKey(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = "CarRentalSystem",
                Audience = "CarRentalSystemUsers"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
