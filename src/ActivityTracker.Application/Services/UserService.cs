using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Interfaces;
using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserDto>> GetAllActiveAsync()
    {
        var users = await _repository.GetAllActiveAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Name       = dto.Name,
            Email      = dto.Email,
            Department = dto.Department,
            CreatedAt  = DateTime.UtcNow
        };
        return MapToDto(await _repository.CreateAsync(user));
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Name       = dto.Name;
        existing.Email      = dto.Email;
        existing.Department = dto.Department;
        existing.IsActive   = dto.IsActive;

        return MapToDto(await _repository.UpdateAsync(existing));
    }

    public async Task<bool> DeactivateAsync(int id) =>
        await _repository.DeactivateAsync(id);

    private static UserDto MapToDto(User u) =>
        new(u.Id, u.Name, u.Email, u.Department, u.IsActive);
}
