using ActivityTracker.Application.DTOs;

namespace ActivityTracker.Application.Interfaces;

public interface IActivityService
{
    Task<IEnumerable<ActivityDto>> GetAllAsync();
    Task<ActivityDto?> GetByIdAsync(int id);
    Task<ActivityDto> CreateAsync(CreateActivityDto dto);
    Task<ActivityDto?> UpdateAsync(int id, UpdateActivityDto dto);
    Task<bool> DeleteAsync(int id);
}
