using ActivityTracker.Application.DTOs;

namespace ActivityTracker.Application.Interfaces;

public interface IAuthService
{
    Task<(string Username, string Role, int? CompanyId, string? CompanyName)?> ValidateCredentialsAsync(LoginDto dto);
}
