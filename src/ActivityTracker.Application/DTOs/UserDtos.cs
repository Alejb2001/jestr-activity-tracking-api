using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Application.DTOs;

public record UserDto(
    int Id,
    string Name,
    string Email,
    string Department,
    bool IsActive
);

public record CreateUserDto(
    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    string Name,

    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "El formato del email no es v\u00e1lido.")]
    [MaxLength(200, ErrorMessage = "El email no puede superar 200 caracteres.")]
    string Email,

    [MaxLength(100, ErrorMessage = "El departamento no puede superar 100 caracteres.")]
    string Department
);

public record UpdateUserDto(
    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    string Name,

    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "El formato del email no es v\u00e1lido.")]
    [MaxLength(200, ErrorMessage = "El email no puede superar 200 caracteres.")]
    string Email,

    [MaxLength(100, ErrorMessage = "El departamento no puede superar 100 caracteres.")]
    string Department,

    bool IsActive
);
