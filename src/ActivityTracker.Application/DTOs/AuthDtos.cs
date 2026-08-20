using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Application.DTOs;

public record LoginDto(
    [Required] string Username,
    [Required] string Password
);

public record AuthResponseDto(
    string Token,
    string Username,
    string Role,
    DateTime ExpiresAt
);
