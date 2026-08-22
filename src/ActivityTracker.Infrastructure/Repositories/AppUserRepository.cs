using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Infrastructure.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _context;

    public AppUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> GetByUsernameAsync(string username) =>
        await _context.AppUsers
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Username == username);

    public async Task<IEnumerable<AppUser>> GetByCompanyIdAsync(int companyId) =>
        await _context.AppUsers
            .Where(u => u.CompanyId == companyId)
            .OrderBy(u => u.Username)
            .ToListAsync();

    public async Task<AppUser> CreateAsync(AppUser user)
    {
        _context.AppUsers.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var user = await _context.AppUsers.FindAsync(id);
        if (user is null) return false;
        user.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string username) =>
        await _context.AppUsers.AnyAsync(u => u.Username == username);
}
