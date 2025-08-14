using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Models;

namespace ToDoList.Service
{
    public interface ITokenService
    {
        string GenerateToken(AppUser user);
        ClaimsPrincipal? ValidateToken(string token);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration config, ILogger<TokenService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GenerateToken(AppUser user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));


                var claims = new List<Claim>
                {
                    new Claim("userId", user.Id),
                    new Claim("email", user.Email ?? ""),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expirationMinutes = int.Parse(_config["JWT:ExpirationInMinutes"] ?? "60");
                var token = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                );

                _logger.LogInformation("JWT token generated successfully for user {Email}", user.Email);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user {Email}", user?.Email);
                throw;
            }
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
                var key = Encoding.UTF8.GetBytes(secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["JWT:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
        }
    }
}
