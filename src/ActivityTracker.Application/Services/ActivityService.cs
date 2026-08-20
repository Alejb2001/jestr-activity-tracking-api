using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Interfaces;
using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Queries;

namespace ActivityTracker.Application.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _repository;

    public ActivityService(IActivityRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ActivityDto>> GetAllAsync(ActivityQueryParams queryParams)
    {
        var (items, totalCount) = await _repository.GetAllAsync(queryParams);
        var totalPages = (int)Math.Ceiling(totalCount / (double)queryParams.PageSize);

        return new PagedResult<ActivityDto>(
            items.Select(MapToDto),
            totalCount,
            queryParams.Page,
            queryParams.PageSize,
            totalPages
        );
    }

    public async Task<ActivityDto?> GetByIdAsync(int id)
    {
        var activity = await _repository.GetByIdAsync(id);
        return activity is null ? null : MapToDto(activity);
    }

    public async Task<ActivityDto> CreateAsync(CreateActivityDto dto)
    {
        var activity = new Activity
        {
            Title          = dto.Title,
            Description    = dto.Description,
            ScheduledStart = dto.ScheduledStart,
            ScheduledEnd   = dto.ScheduledEnd,
            AssignedUserId = dto.AssignedUserId,
            CreatedAt      = DateTime.UtcNow
        };
        return MapToDto(await _repository.CreateAsync(activity));
    }

    public async Task<ActivityDto?> UpdateAsync(int id, UpdateActivityDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Title          = dto.Title;
        existing.Description    = dto.Description;
        existing.ScheduledStart = dto.ScheduledStart;
        existing.ScheduledEnd   = dto.ScheduledEnd;
        existing.Status         = dto.Status;
        existing.AssignedUserId = dto.AssignedUserId;
        existing.UpdatedAt      = DateTime.UtcNow;

        return MapToDto(await _repository.UpdateAsync(existing));
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _repository.DeleteAsync(id);

    private static ActivityDto MapToDto(Activity a) => new(
        a.Id, a.Title, a.Description, a.ScheduledStart, a.ScheduledEnd,
        a.Status, a.Status.ToString(), a.AssignedUserId, a.CreatedAt, a.UpdatedAt
    );
}
