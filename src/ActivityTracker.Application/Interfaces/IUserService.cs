using ActivityTracker.Application.DTOs;

namespace ActivityTracker.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllActiveAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto);
    Task<bool> DeactivateAsync(int id);
}
