﻿using System.Security.Claims;

namespace FashionWeb.Domain.Services.Jwts
{
    public interface IJwtTokenService
    {
        Task<bool> ValidateToken(string token);
        Task<List<Claim>> GetClaims(string token);
    }
}
