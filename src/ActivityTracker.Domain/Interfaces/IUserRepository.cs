using ActivityTracker.Domain.Entities;

namespace ActivityTracker.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllActiveAsync(int? companyId);
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeactivateAsync(int id);
}
