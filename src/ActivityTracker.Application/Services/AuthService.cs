using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Interfaces;

namespace ActivityTracker.Application.Services;

public class AuthService : IAuthService
{
    // Demo users: in a real app these would come from a database with hashed passwords.
    private static readonly Dictionary<string, (string Password, string Role)> _users = new()
    {
        { "admin",  ("Admin123!",  "Admin") },
        { "viewer", ("Viewer123!", "Viewer") }
    };

    public (string Username, string Role)? ValidateCredentials(LoginDto dto)
    {
        if (!_users.TryGetValue(dto.Username.ToLower(), out var userData))
            return null;

        if (userData.Password != dto.Password)
            return null;

        return (dto.Username.ToLower(), userData.Role);
    }
}
