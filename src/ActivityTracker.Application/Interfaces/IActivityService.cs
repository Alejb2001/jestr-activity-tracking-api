using ActivityTracker.Application.DTOs;
using ActivityTracker.Domain.Queries;

namespace ActivityTracker.Application.Interfaces;

public interface IActivityService
{
    Task<PagedResult<ActivityDto>> GetAllAsync(ActivityQueryParams queryParams, int? companyId);
    Task<ActivityDto?> GetByIdAsync(int id);
    Task<ActivityDto> CreateAsync(CreateActivityDto dto);
    Task<ActivityDto?> UpdateAsync(int id, UpdateActivityDto dto);
    Task<bool> DeleteAsync(int id);
}
