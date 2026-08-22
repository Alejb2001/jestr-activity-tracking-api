using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Helpers;
using ActivityTracker.Application.Interfaces;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAppUserRepository _users;

    public AuthService(IAppUserRepository users)
    {
        _users = users;
    }

    public async Task<(string Username, string Role, int? CompanyId, string? CompanyName)?> ValidateCredentialsAsync(LoginDto dto)
    {
        var user = await _users.GetByUsernameAsync(dto.Username.Trim().ToLower());
        if (user is null || !user.IsActive) return null;

        // Company validation: non-global roles must supply the correct company code
        var isGlobal = user.Role is "admin" or "viewer";
        if (!isGlobal)
        {
            if (string.IsNullOrWhiteSpace(dto.CompanyCode)) return null;
            if (user.Company is null || !string.Equals(user.Company.Code, dto.CompanyCode.Trim(), StringComparison.OrdinalIgnoreCase))
                return null;
            if (!user.Company.IsActive) return null;
        }

        if (!PasswordHelper.Verify(dto.Password, user.PasswordHash)) return null;

        return (user.Username, user.Role, user.CompanyId, user.Company?.Name);
    }
}
