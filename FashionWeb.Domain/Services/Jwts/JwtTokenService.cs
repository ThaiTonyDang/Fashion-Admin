using FashionWeb.Domain.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FashionWeb.Domain.Services.Jwts
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly TokenConfig _tokenConfig;

        public JwtTokenService(IOptions<TokenConfig> options)
        {
            this._tokenConfig = options.Value;
        }
        public Task<bool> ValidateToken(string token)
        {
            var tokenOptions = GetTokenOptions();
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var result = tokenHandler.ValidateToken(token, tokenOptions, out _);
                var isAuthenticated = result.Identity != null && result.Identity.IsAuthenticated;
                return Task.FromResult(isAuthenticated);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<List<Claim>> GetClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();
            claims.Add(new Claim("token", token));
            return Task.FromResult(claims);
        }

        private TokenValidationParameters GetTokenOptions()
        {
            var tokenOptions = new TokenValidationParameters()
            {
                ValidIssuer = _tokenConfig.Issuer,
                ValidAudience = _tokenConfig.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret))
            };

            return tokenOptions;
        }
    }
}
