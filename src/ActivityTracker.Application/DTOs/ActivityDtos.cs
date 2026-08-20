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
    string Title,
    string Description,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    string AssignedUserId
);

public record UpdateActivityDto(
    string Title,
    string Description,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    ActivityStatus Status,
    string AssignedUserId
);
