using ActivityTracker.Domain.Entities;

namespace ActivityTracker.Domain.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(int id);
    Task<Company?> GetByCodeAsync(string code);
    Task<Company> CreateAsync(Company company);
    Task<Company> UpdateAsync(Company company);
    Task<bool> DeactivateAsync(int id);
}
