using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Application.DTOs;

public record LoginDto(
    [Required] string Username,
    [Required] string Password,
    string? CompanyCode
);

public record AuthResponseDto(
    string Token,
    string Username,
    string Role,
    int? CompanyId,
    string? CompanyName,
    DateTime ExpiresAt
);
