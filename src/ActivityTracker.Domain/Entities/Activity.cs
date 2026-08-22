using ActivityTracker.Domain.Enums;

namespace ActivityTracker.Domain.Entities;

public class Activity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledStart { get; set; }
    public DateTime ScheduledEnd { get; set; }
    public ActivityStatus Status { get; set; } = ActivityStatus.Pending;
    public ActivityPriority Priority { get; set; } = ActivityPriority.Medium;
    public string AssignedUserId { get; set; } = string.Empty;
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
