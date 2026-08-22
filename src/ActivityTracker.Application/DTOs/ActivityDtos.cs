using System.ComponentModel.DataAnnotations;
using ActivityTracker.Domain.Enums;

namespace ActivityTracker.Application.DTOs;

public record ActivityDto(
    int Id,
    string Title,
    string Description,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    ActivityStatus Status,
    string StatusLabel,
    string AssignedUserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateActivityDto(
    [Required(ErrorMessage = "El título es requerido.")]
    [MinLength(3,  ErrorMessage = "El título debe tener al menos 3 caracteres.")]
    [MaxLength(100, ErrorMessage = "El título no puede superar 100 caracteres.")]
    string Title,

    [Required(ErrorMessage = "La descripción es requerida.")]
    [MaxLength(1000, ErrorMessage = "La descripción no puede superar 1000 caracteres.")]
    string Description,

    [Required(ErrorMessage = "La fecha de inicio es requerida.")]
    DateTime ScheduledStart,

    [Required(ErrorMessage = "La fecha de conclusión es requerida.")]
    DateTime ScheduledEnd,

    [Required(ErrorMessage = "El responsable es requerido.")]
    [MaxLength(100, ErrorMessage = "El ID del responsable no puede superar 100 caracteres.")]
    string AssignedUserId,

    int? CompanyId
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ScheduledEnd <= ScheduledStart)
            yield return new ValidationResult(
                "La fecha de conclusión debe ser posterior a la fecha de inicio.",
                new[] { nameof(ScheduledEnd) });
    }
}

public record UpdateActivityDto(
    [Required(ErrorMessage = "El título es requerido.")]
    [MinLength(3,  ErrorMessage = "El título debe tener al menos 3 caracteres.")]
    [MaxLength(100, ErrorMessage = "El título no puede superar 100 caracteres.")]
    string Title,

    [Required(ErrorMessage = "La descripción es requerida.")]
    [MaxLength(1000, ErrorMessage = "La descripción no puede superar 1000 caracteres.")]
    string Description,

    [Required(ErrorMessage = "La fecha de inicio es requerida.")]
    DateTime ScheduledStart,

    [Required(ErrorMessage = "La fecha de conclusión es requerida.")]
    DateTime ScheduledEnd,

    [EnumDataType(typeof(ActivityStatus), ErrorMessage = "Estado de actividad no válido.")]
    ActivityStatus Status,

    [Required(ErrorMessage = "El responsable es requerido.")]
    [MaxLength(100, ErrorMessage = "El ID del responsable no puede superar 100 caracteres.")]
    string AssignedUserId
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ScheduledEnd <= ScheduledStart)
            yield return new ValidationResult(
                "La fecha de conclusión debe ser posterior a la fecha de inicio.",
                new[] { nameof(ScheduledEnd) });
    }
}
