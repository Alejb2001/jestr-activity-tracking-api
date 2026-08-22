using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Application.DTOs;

public record CompanyDto(
    int Id,
    string Name,
    string Code,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateCompanyDto(
    [Required][MaxLength(100)] string Name,
    [Required][MaxLength(20)] string Code
);

public record UpdateCompanyDto(
    [Required][MaxLength(100)] string Name,
    bool IsActive
);

public record CompanyUserDto(
    int Id,
    string Username,
    string Name,
    string Email,
    string Department,
    string Role,
    bool IsActive
);

public record CreateCompanyUserDto(
    [Required][MaxLength(50)] string Username,
    [Required][MinLength(8)] string Password,
    [Required][MaxLength(100)] string Name,
    [Required][EmailAddress][MaxLength(200)] string Email,
    [MaxLength(100)] string Department,
    [Required] string Role   // "company_admin" | "company_viewer"
);
