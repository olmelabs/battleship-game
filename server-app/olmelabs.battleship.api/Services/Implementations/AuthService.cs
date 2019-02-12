using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services.Interfaces;

/*
 * Some credits on refresh token
 * https://www.blinkingcaret.com/2018/05/30/refresh-tokens-in-asp-net-core-web-api/
 */
namespace olmelabs.battleship.api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IStorage _storage;
        private readonly IConfiguration _config;

        public AuthService(IStorage storage, IConfiguration config)
        {
            _storage = storage;
            _config = config;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user  = await _storage.FindUserAsync(email);
            if (user == null)
                return null;

            PasswordHasher<User> pw = new PasswordHasher<User>();
            PasswordVerificationResult result  = pw.VerifyHashedPassword(user, user.PasswordHash, password);
            bool pwdOk = result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;

            if (pwdOk && user.IsEmailConfirmed)
            {
                return user;
            }
            return null;
        }

        public Task<User> FindUserAsync(string email)
        {
            return _storage.FindUserAsync(email);
        }

        public string BuildJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            int timeToLive = int.TryParse(_config["Jwt:TokenLifeHours"], out timeToLive) ? timeToLive : 24;

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddHours(timeToLive),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string BuildRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<string> GetEmailByRefreshTokenAsync(string refreshToken)
        {
            var rt = await _storage.GetEmailByRefreshTokenAsync(refreshToken);

            if (rt == null)
                return null;

            if (rt.ExpirationDate > DateTime.Now)
                return rt.Email;
            else
                await DeleteRefreshTokenAsync(refreshToken);

            return null;
        }

        public Task AddRefreshTokenAsync(string refreshToken, string email)
        {
            RefreshToken rt = new RefreshToken { Token = refreshToken, Email = email, ExpirationDate = DateTime.Now.AddDays(30) };
            return _storage.AddRefreshTokenAsync(rt);
        }

        public Task DeleteRefreshTokenAsync(string refreshToken)
        {
            return _storage.DeleteRefreshTokenAsync(refreshToken);
        }
    }
}
