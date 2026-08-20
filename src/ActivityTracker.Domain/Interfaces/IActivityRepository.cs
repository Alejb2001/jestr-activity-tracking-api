using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Queries;

namespace ActivityTracker.Domain.Interfaces;

public interface IActivityRepository
{
    Task<(IEnumerable<Activity> Items, int TotalCount)> GetAllAsync(ActivityQueryParams queryParams);
    Task<Activity?> GetByIdAsync(int id);
    Task<Activity> CreateAsync(Activity activity);
    Task<Activity> UpdateAsync(Activity activity);
    Task<bool> DeleteAsync(int id);
}
