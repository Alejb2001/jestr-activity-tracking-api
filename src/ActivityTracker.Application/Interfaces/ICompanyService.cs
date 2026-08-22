using ActivityTracker.Application.DTOs;

namespace ActivityTracker.Application.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllAsync();
    Task<CompanyDto?> GetByIdAsync(int id);
    Task<CompanyDto> CreateAsync(CreateCompanyDto dto);
    Task<CompanyDto?> UpdateAsync(int id, UpdateCompanyDto dto);
    Task<bool> DeactivateAsync(int id);

    Task<IEnumerable<CompanyUserDto>> GetUsersAsync(int companyId);
    Task<CompanyUserDto> CreateUserAsync(int companyId, CreateCompanyUserDto dto);
    Task<bool> DeactivateUserAsync(int companyId, int userId);
}
