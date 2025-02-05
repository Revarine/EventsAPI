using Events.Domain.Entities;

namespace Events.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    bool ValidateAccessToken(string token);
    bool ValidateRefreshToken(string token);
    Guid? GetUserIdFromToken(string token);
    bool IsUserAdmin(string token);
} 