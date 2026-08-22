using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Helpers;
using ActivityTracker.Application.Interfaces;
using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companies;
    private readonly IAppUserRepository _appUsers;
    private readonly IUserRepository _users;

    public CompanyService(ICompanyRepository companies, IAppUserRepository appUsers, IUserRepository users)
    {
        _companies = companies;
        _appUsers = appUsers;
        _users = users;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var list = await _companies.GetAllAsync();
        return list.Select(MapToDto);
    }

    public async Task<CompanyDto?> GetByIdAsync(int id)
    {
        var c = await _companies.GetByIdAsync(id);
        return c is null ? null : MapToDto(c);
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto)
    {
        var company = new Company
        {
            Name      = dto.Name.Trim(),
            Code      = dto.Code.Trim().ToUpper(),
            CreatedAt = DateTime.UtcNow
        };
        return MapToDto(await _companies.CreateAsync(company));
    }

    public async Task<CompanyDto?> UpdateAsync(int id, UpdateCompanyDto dto)
    {
        var existing = await _companies.GetByIdAsync(id);
        if (existing is null) return null;
        existing.Name     = dto.Name.Trim();
        existing.IsActive = dto.IsActive;
        return MapToDto(await _companies.UpdateAsync(existing));
    }

    public async Task<bool> DeactivateAsync(int id) =>
        await _companies.DeactivateAsync(id);

    public async Task<IEnumerable<CompanyUserDto>> GetUsersAsync(int companyId)
    {
        var appUsers = await _appUsers.GetByCompanyIdAsync(companyId);
        return appUsers.Select(MapUserToDto);
    }

    public async Task<CompanyUserDto> CreateUserAsync(int companyId, CreateCompanyUserDto dto)
    {
        var validRoles = new[] { "company_admin", "company_viewer" };
        if (!validRoles.Contains(dto.Role))
            throw new ArgumentException($"Rol inválido: {dto.Role}");

        var username = dto.Username.Trim().ToLower();
        if (await _appUsers.ExistsAsync(username))
            throw new InvalidOperationException("El nombre de usuario ya está en uso.");

        var name  = dto.Name.Trim();
        var email = dto.Email.Trim();
        var dept  = dto.Department?.Trim() ?? string.Empty;

        // Create login account (stores profile info for display)
        var appUser = new AppUser
        {
            Username     = username,
            PasswordHash = PasswordHelper.Hash(dto.Password),
            Role         = dto.Role,
            Name         = name,
            Email        = email,
            Department   = dept,
            CompanyId    = companyId,
            CreatedAt    = DateTime.UtcNow
        };
        var created = await _appUsers.CreateAsync(appUser);

        // Create responsible (for activity assignment dropdown)
        var responsible = new User
        {
            Name       = name,
            Email      = email,
            Department = dept,
            CompanyId  = companyId,
            CreatedAt  = DateTime.UtcNow
        };
        await _users.CreateAsync(responsible);

        return MapUserToDto(created);
    }

    public async Task<bool> DeactivateUserAsync(int companyId, int userId)
    {
        var users = await _appUsers.GetByCompanyIdAsync(companyId);
        if (!users.Any(u => u.Id == userId)) return false;
        return await _appUsers.DeactivateAsync(userId);
    }

    private static CompanyDto MapToDto(Company c) =>
        new(c.Id, c.Name, c.Code, c.IsActive, c.CreatedAt);

    private static CompanyUserDto MapUserToDto(AppUser u) =>
        new(u.Id, u.Username, u.Name, u.Email, u.Department, u.Role, u.IsActive);
}
