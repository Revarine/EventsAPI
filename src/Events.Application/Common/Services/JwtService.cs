using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Events.Application.Common.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _accessTokenSecret;
    private readonly string _refreshTokenSecret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        var jwtSettings = _configuration.GetSection("JwtSettings");
        _accessTokenSecret = jwtSettings["AccessTokenSecret"]!;
        _refreshTokenSecret = jwtSettings["RefreshTokenSecret"]!;
        _issuer = jwtSettings["Issuer"]!;
        _audience = jwtSettings["Audience"]!;
        _accessTokenExpirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);
        _refreshTokenExpirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"]!);
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.isAdmin ? "Admin" : "User")
        };

        return GenerateToken(claims, _accessTokenSecret, TimeSpan.FromMinutes(_accessTokenExpirationMinutes));
    }

    public string GenerateRefreshToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("TokenType", "Refresh")
        };

        return GenerateToken(claims, _refreshTokenSecret, TimeSpan.FromDays(_refreshTokenExpirationDays));
    }

    public bool ValidateAccessToken(string token)
    {
        return ValidateToken(token, _accessTokenSecret);
    }

    public bool ValidateRefreshToken(string token)
    {
        if (!ValidateToken(token, _refreshTokenSecret))
            return false;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var tokenType = jwtToken.Claims.FirstOrDefault(x => x.Type == "TokenType")?.Value;
            return tokenType == "Refresh";
        }
        catch
        {
            return false;
        }
    }

    public Guid? GetUserIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(userId);
        }
        catch
        {
            return null;
        }
    }

    public bool IsUserAdmin(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_accessTokenSecret);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            return role == "Admin";
        }
        catch
        {
            return false;
        }
    }

    private string GenerateToken(IEnumerable<Claim> claims, string secret, TimeSpan expiration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(expiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool ValidateToken(string token, string secret)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }
} 