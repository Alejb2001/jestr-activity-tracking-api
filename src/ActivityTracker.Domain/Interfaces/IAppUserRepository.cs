using ActivityTracker.Domain.Entities;

namespace ActivityTracker.Domain.Interfaces;

public interface IAppUserRepository
{
    Task<AppUser?> GetByUsernameAsync(string username);
    Task<IEnumerable<AppUser>> GetByCompanyIdAsync(int companyId);
    Task<AppUser> CreateAsync(AppUser user);
    Task<bool> DeactivateAsync(int id);
    Task<bool> ExistsAsync(string username);
}
