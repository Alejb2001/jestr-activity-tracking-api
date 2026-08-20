using ActivityTracker.Domain.Entities;

namespace ActivityTracker.Domain.Interfaces;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetAllAsync();
    Task<Activity?> GetByIdAsync(int id);
    Task<Activity> CreateAsync(Activity activity);
    Task<Activity> UpdateAsync(Activity activity);
    Task<bool> DeleteAsync(int id);
}
