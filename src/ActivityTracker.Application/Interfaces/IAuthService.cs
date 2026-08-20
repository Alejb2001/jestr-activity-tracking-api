using ActivityTracker.Application.DTOs;

namespace ActivityTracker.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Validates credentials. Returns (Username, Role) on success, or null on failure.
    /// </summary>
    (string Username, string Role)? ValidateCredentials(LoginDto dto);
}
