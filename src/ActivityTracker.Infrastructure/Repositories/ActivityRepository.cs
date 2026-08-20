using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Infrastructure.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _context;

    public ActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync() =>
        await _context.Activities.OrderByDescending(a => a.CreatedAt).ToListAsync();

    public async Task<Activity?> GetByIdAsync(int id) =>
        await _context.Activities.FindAsync(id);

    public async Task<Activity> CreateAsync(Activity activity)
    {
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<Activity> UpdateAsync(Activity activity)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity is null) return false;
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
        return true;
    }
}
