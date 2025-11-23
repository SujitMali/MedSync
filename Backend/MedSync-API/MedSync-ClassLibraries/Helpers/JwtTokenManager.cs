using MedSync_ClassLibraries.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedSync_ClassLibraries.Helpers
{
    public static class JwtTokenManager
    {

        private static readonly string secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];
        private static readonly int expiryMinutes = int.Parse(ConfigurationManager.AppSettings["JwtExpiryMinutes"]);

        public static string GenerateToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.RoleName ?? string.Empty),
                new Claim("DoctorID", user.DoctorID.HasValue ? user.DoctorID.Value.ToString() : string.Empty),
                new Claim("IsActive", user.IsActive ? "true" : "false")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);

                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                return tokenHandler.ValidateToken(token, validationParams, out validatedToken);
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
                return null;
            }
        }

    }
}
